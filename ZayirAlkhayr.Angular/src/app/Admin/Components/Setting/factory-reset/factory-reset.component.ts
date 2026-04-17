import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { firstValueFrom } from 'rxjs';
import { AdminService } from 'src/app/Admin/Services/admin.service';

interface FactoryResetPreviewItem {
  key: string;
  displayName: string;
  description: string;
  recordsCount: number;
  executionOrder: number;
}

interface FactoryResetStepResult {
  key: string;
  displayName: string;
  statusMessage: string;
  deletedRecordsCount: number;
  remainingRecordsCount: number;
  executionOrder: number;
}

type StepStatus = 'idle' | 'running' | 'completed' | 'failed';
type LogTone = 'info' | 'success' | 'error';

interface FactoryResetStepVm extends FactoryResetPreviewItem {
  deletedRecordsCount: number;
  status: StepStatus;
  apiMessage: string;
}

interface FactoryResetLogEntry {
  tone: LogTone;
  message: string;
  timestamp: string;
}

@Component({
  selector: 'app-factory-reset',
  templateUrl: './factory-reset.component.html',
  styleUrls: ['./factory-reset.component.css']
})
export class FactoryResetComponent implements OnInit {
  readonly confirmationPhrase = 'RESET';

  showLoader = false;
  isResetting = false;
  currentActionText = 'راجع البيانات أولًا ثم ابدأ إعادة الضبط إذا كنت متأكدًا من الحذف.';
  lastApiResponse = '';
  confirmResetForm: FormGroup;
  steps: FactoryResetStepVm[] = [];
  activityLogs: FactoryResetLogEntry[] = [];

  constructor(
    private adminService: AdminService,
    private fb: FormBuilder,
    private modalService: NgbModal,
    private toaster: ToastrService
  ) { }

  ngOnInit(): void {
    this.initConfirmForm();
    this.loadPreview();
  }

  get totalStepsCount(): number {
    return this.steps.length;
  }

  get completedStepsCount(): number {
    return this.steps.filter(step => step.status === 'completed').length;
  }

  get totalRecordsCount(): number {
    return this.steps.reduce((total, step) => total + step.recordsCount, 0);
  }

  get deletedRecordsTotal(): number {
    return this.steps.reduce((total, step) => total + step.deletedRecordsCount, 0);
  }

  get progressPercent(): number {
    if (!this.totalStepsCount) {
      return 0;
    }

    return Math.round((this.completedStepsCount / this.totalStepsCount) * 100);
  }

  get canStartReset(): boolean {
    return !this.isResetting && this.steps.length > 0;
  }

  get hasDataToDelete(): boolean {
    return this.totalRecordsCount > 0;
  }

  openConfirmationModal(content: any): void {
    if (!this.canStartReset) {
      return;
    }

    this.confirmResetForm.reset({
      confirmDeletion: false,
      confirmationText: ''
    });

    this.modalService.open(content, {
      size: 'lg',
      centered: true,
      scrollable: true
    });
  }

  async startFactoryReset(): Promise<void> {
    if (this.confirmResetForm.invalid) {
      this.confirmResetForm.markAllAsTouched();
      return;
    }

    this.modalService.dismissAll();
    this.isResetting = true;
    this.lastApiResponse = '';
    this.activityLogs = [];
    this.resetStepStatuses();

    if (!this.steps.length) {
      this.isResetting = false;
      return;
    }

    for (const step of this.steps) {
      step.status = 'running';
      step.apiMessage = '';
      this.currentActionText = `يتم الآن حذف بيانات ${step.displayName}...`;
      this.pushLog('info', this.currentActionText);

      try {
        const response = await firstValueFrom(this.adminService.DeleteFactoryResetTarget(step.key));

        if (!response?.isSuccess || !response.results) {
          const failedMessage = response?.message || `تعذر حذف بيانات ${step.displayName}.`;
          step.status = 'failed';
          step.apiMessage = failedMessage;
          this.lastApiResponse = failedMessage;
          this.currentActionText = `توقفت العملية أثناء حذف ${step.displayName}.`;
          this.pushLog('error', failedMessage);
          this.toaster.error(failedMessage);
          this.isResetting = false;
          return;
        }

        const result = response.results as FactoryResetStepResult;
        step.status = 'completed';
        step.deletedRecordsCount = result.deletedRecordsCount ?? 0;
        step.recordsCount = result.remainingRecordsCount ?? 0;
        step.apiMessage = result.statusMessage || response.message || `تم حذف ${step.displayName} بنجاح.`;
        this.lastApiResponse = step.apiMessage;
        this.currentActionText = `تم الانتهاء من حذف ${step.displayName}.`;
        this.pushLog('success', step.apiMessage);
      } catch {
        const errorMessage = `حدث خطأ أثناء حذف بيانات ${step.displayName}.`;
        step.status = 'failed';
        step.apiMessage = errorMessage;
        this.lastApiResponse = errorMessage;
        this.currentActionText = `توقفت العملية أثناء حذف ${step.displayName}.`;
        this.pushLog('error', errorMessage);
        this.toaster.error(errorMessage);
        this.isResetting = false;
        return;
      }
    }

    this.currentActionText = 'اكتملت عملية إعادة ضبط المصنع بنجاح.';
    this.lastApiResponse = 'تم حذف جميع البيانات المستهدفة بنجاح.';
    this.pushLog('success', this.lastApiResponse);
    this.toaster.success(this.lastApiResponse);
    this.isResetting = false;
  }

  private initConfirmForm(): void {
    this.confirmResetForm = this.fb.group({
      confirmDeletion: [false, Validators.requiredTrue],
      confirmationText: ['', [Validators.required, Validators.pattern(`^${this.confirmationPhrase}$`)]]
    });
  }

  private loadPreview(showLoader = true): void {
    this.showLoader = showLoader;

    this.adminService.GetFactoryResetPreview().subscribe({
      next: (response) => {
        this.showLoader = false;

        if (!response?.isSuccess) {
          this.toaster.error(response?.message || 'تعذر تحميل معاينة إعادة الضبط.');
          return;
        }

        const results = (response.results ?? []) as FactoryResetPreviewItem[];
        this.steps = results
          .sort((first, second) => first.executionOrder - second.executionOrder)
          .map((step) => ({
            ...step,
            deletedRecordsCount: 0,
            status: 'idle',
            apiMessage: ''
          }));

        if (!this.isResetting) {
          this.currentActionText = this.hasDataToDelete
            ? 'كل مرحلة ستُنفذ بالتتابع مع عرض الرد المباشر من الخادم.'
            : 'لا توجد سجلات مستهدفة للحذف حاليًا داخل الجداول المحددة.';
          this.lastApiResponse = '';
        }
      },
      error: () => {
        this.showLoader = false;
        this.toaster.error('حدث خطأ أثناء تحميل بيانات Factory Reset.');
      }
    });
  }

  private resetStepStatuses(): void {
    this.steps = this.steps.map((step) => ({
      ...step,
      status: 'idle',
      deletedRecordsCount: 0,
      apiMessage: ''
    }));
  }

  private pushLog(tone: LogTone, message: string): void {
    this.activityLogs.unshift({
      tone,
      message,
      timestamp: new Date().toLocaleTimeString('en-GB')
    });
  }
}
