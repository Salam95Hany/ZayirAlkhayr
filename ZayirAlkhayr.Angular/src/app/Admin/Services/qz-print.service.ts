import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import html2canvas from 'html2canvas';
import qz from 'qz-tray';
import { ReceiptItemsModel, ReceiptModel } from '../Models/ReceiptModel';

@Injectable({
  providedIn: 'root'
})
export class QzPrintService {
  private readonly paperWidthMm = 80;
  private readonly printableWidthMm = 72;
  private readonly printerDensityDpmm = 8;
  private readonly receiptWidth = this.printableWidthMm * this.printerDensityDpmm;
  private readonly logoPath = 'assets/POS_Logo3.png';
  private readonly preferredPrinterKeywords = ['xp-q810k', 'xprinter'];
  private readonly storeName = 'صبح و مسا';
  private readonly storePhones = '0998222283 - 0998222286';
  private readonly storeAddress = 'درعا جاسم شرق المركز الثقافي 200 م';
  private logoBase64 = '';
  private logoLoadPromise: Promise<void>;
  private qzConnectionPromise?: Promise<void>;

  constructor(private toaster: ToastrService) {
    this.logoLoadPromise = this.loadLogo();
  }

  async Print(result: ReceiptModel): Promise<void> {
    let container: HTMLDivElement | null = null;

    try {
      await this.logoLoadPromise;
      await this.InitQZ();

      const printer = await this.resolvePrinterName();
      container = this.createPrintContainer(result);
      document.body.appendChild(container);

      await this.waitForContainerReady(container);

      const canvas = await this.renderReceipt(container);
      const config = this.createReceiptConfig(printer, this.calculatePaperHeight(canvas.height));

      await qz.print(config, [this.buildReceiptImage(canvas)]);
    } catch (error) {
      const message = this.showPrintError(error);
      const printableError = error instanceof Error ? error : new Error(message);

      (printableError as Error & { userMessage?: string }).userMessage = message;
      throw printableError;
    } finally {
      container?.remove();
    }
  }

  GetPrinters() {
    return this.InitQZ().then(() => qz.printers.find());
  }

  GetDefaultPrinter() {
    return this.InitQZ().then(() => qz.printers.getDefault());
  }

  handlePrintError(error: any): string {
    const msg = `${error?.message || error || ''}`.toLowerCase();

    if (
      msg.includes('websocket')
      || msg.includes('connection')
      || msg.includes('closed before')
      || msg.includes('establish')
      || msg.includes('qz tray')
      || msg.includes('refused')
      || msg.includes('failed to fetch')
    ) {
      return '❌ برنامج QZ Tray غير شغال. يرجى تشغيله';
    }

    if (
      msg.includes('certificate')
      || msg.includes('allow')
      || msg.includes('blocked')
      || msg.includes('signature')
      || msg.includes('signing')
    ) {
      return '❌ مشكلة في صلاحيات الطباعة. يرجى الموافقة على Allow';
    }

    if (
      msg.includes('printer')
      || msg.includes('no printers')
      || msg.includes('not found')
    ) {
      return '❌ لم يتم العثور على الطابعة. تأكد من توصيلها';
    }

    if (
      msg.includes('timeout')
      || msg.includes('timed out')
      || msg.includes('not responding')
    ) {
      return '❌ الطابعة لا تستجيب. تأكد أنها شغالة وبها ورق';
    }

    return '❌ حدث خطأ أثناء الطباعة. حاول مرة أخرى';
  }

  BuildReceiptBody(order: ReceiptModel): string {
    const items = order.items ?? [];
    const itemsTotal = this.getItemsTotal(items);
    const deliveryFee = Number(order.deliveryFee ?? 0);
    const grandTotal = Number(order.grandTotal ?? itemsTotal + deliveryFee);
    const itemsCount = items.reduce((sum, item) => sum + Number(item.qty ?? 0), 0);
    const customerHtml = order.agent ? this.buildInfoRow('العميل', order.agent) : '';
    const deliveryFeeHtml = deliveryFee > 0
      ? this.buildSummaryRow('رسوم التوصيل', `${this.formatAmount(deliveryFee)} ل.س`)
      : '';

    return `
      <div style="width:${this.receiptWidth}px;background:#fff;color:#000;box-sizing:border-box;">
        <div style="
          width:100%;
          padding:12px 14px 24px;
          box-sizing:border-box;
          font-family:'Tahoma', 'Arial', 'Segoe UI', sans-serif;
          direction:rtl;
          text-align:right;
          line-height:1.5;
          background:#fff;
        ">
          <div style="border:2px solid #000;border-radius:14px;padding:14px 12px 12px;background:#fff;">
            ${this.logoBase64 ? `
              <div style="display:flex;justify-content:center;align-items:center;">
                <div style="width:104px;height:104px;border:1.5px solid #000;border-radius:50%;display:flex;justify-content:center;align-items:center;padding:10px;box-sizing:border-box;">
                  <img
                    src="${this.logoBase64}"
                    alt="logo"
                    style="width:82px;height:82px;object-fit:contain;"
                  />
                </div>
              </div>
            ` : ''}

            <div style="margin-top:10px;text-align:center;">
              <div style="
                font-size:30px;
                font-weight:800;
                line-height:1.2;
                letter-spacing:0;
                word-spacing:0;
                direction:rtl;
                unicode-bidi:isolate;
                display:block;
              ">
                ${this.escapeHtml(this.storeName)}
              </div>
              <div style="font-size:12px;margin-top:8px;">الهاتف: ${this.escapeHtml(this.storePhones)}</div>
              <div style="font-size:12px;margin-top:3px;word-break:break-word;">العنوان: ${this.escapeHtml(this.storeAddress)}</div>
            </div>
          </div>

          <div style="margin-top:10px;border:1.5px solid #000;border-radius:10px;padding:10px;background:#fff;">
            <div style="font-size:13px;font-weight:800;text-align:center;background:#f3f3f3;border:1px solid #000;border-radius:8px;padding:5px 8px;margin-bottom:8px;">
              بيانات الطلب
            </div>
            ${this.buildInfoRow('رقم الطلب', order.orderNo)}
            ${this.buildInfoRow('نوع الطلب', order.orderType)}
            ${this.buildInfoRow('التاريخ', order.date)}
            ${this.buildInfoRow('الكاشير', order.cashier)}
            ${customerHtml}
          </div>

          <div style="margin-top:10px;">
            <div style="display:flex;justify-content:space-between;align-items:center;margin-bottom:6px;padding:0 2px;font-size:14px;font-weight:800;">
              <span>تفاصيل الأصناف</span>
              <span>${this.escapeHtml(itemsCount)} صنف</span>
            </div>
            ${this.buildItemsTable(items)}
          </div>

          <div style="margin-top:10px;border:2px solid #000;border-radius:10px;padding:10px 12px;background:#fff;">
            <div style="font-size:13px;font-weight:800;text-align:center;background:#f3f3f3;border:1px solid #000;border-radius:8px;padding:5px 8px;margin-bottom:8px;">
              ملخص الحساب
            </div>
            ${this.buildSummaryRow('إجمالي الأصناف', `${this.formatAmount(itemsTotal)} ل.س`)}
            ${deliveryFeeHtml}
            <div style="border-top:1px dashed #000;margin:8px 0 6px;"></div>
            ${this.buildSummaryRow('الإجمالي النهائي', `${this.formatAmount(grandTotal)} ل.س`, true)}
          </div>

          <div style="margin-top:12px;border-top:1px dashed #000;padding-top:10px;text-align:center;">
            <div style="font-size:12px;font-weight:700;">شكراً لزيارتكم</div>
            <div style="font-size:11px;margin-top:3px;">نتشرف بخدمتكم دائماً</div>
          </div>
        </div>
      </div>
    `;
  }

  GroupItemsByCategory(items: ReceiptItemsModel[]) {
    const grouped: { [key: string]: ReceiptItemsModel[] } = {};

    items.forEach(item => {
      const key = String(item.categoryId ?? 'uncategorized');
      if (!grouped[key]) {
        grouped[key] = [];
      }

      grouped[key].push(item);
    });

    return grouped;
  }

  private async loadLogo(): Promise<void> {
    try {
      const response = await fetch(this.logoPath);
      if (!response.ok) {
        throw new Error(`Failed to load logo: ${response.status} ${response.statusText}`);
      }

      const blob = await response.blob();
      this.logoBase64 = await this.blobToBase64(blob);
    } catch (error) {
      this.logoBase64 = '';
      console.error('Logo loading error', error);
    }
  }

  private InitQZ(): Promise<void> {
    if (qz.websocket.isActive()) {
      return Promise.resolve();
    }

    if (!this.qzConnectionPromise) {
      this.qzConnectionPromise = this.connectQz()
        .catch(error => {
          this.qzConnectionPromise = undefined;
          throw error;
        });
    }

    return this.qzConnectionPromise;
  }

  private async connectQz(): Promise<void> {
    const isHttpsPage = typeof window !== 'undefined' && window.location?.protocol === 'https:';
    const secureModes = isHttpsPage ? [true] : [false, true];
    let lastError: unknown;

    for (const usingSecure of secureModes) {
      try {
        await qz.websocket.connect({
          usingSecure,
          keepAlive: 60,
          retries: 1,
          delay: 0.5
        });
        return;
      } catch (error) {
        lastError = error;
      }
    }

    throw lastError;
  }

  private async resolvePrinterName(): Promise<string> {
    const printers = await qz.printers.find();
    const printerList = Array.isArray(printers) ? printers.map(printer => `${printer}`) : [];

    if (!printerList.length) {
      throw new Error('No printers were found in QZ Tray.');
    }

    const preferredPrinter = printerList.find(printer =>
      this.preferredPrinterKeywords.some(keyword => printer.toLowerCase().includes(keyword))
    );

    if (preferredPrinter) {
      return preferredPrinter;
    }

    const defaultPrinter = await qz.printers.getDefault().catch(() => null);
    if (defaultPrinter) {
      return `${defaultPrinter}`;
    }

    return printerList[0];
  }

  private renderReceipt(container: HTMLElement): Promise<HTMLCanvasElement> {
    return html2canvas(container, {
      scale: 1,
      useCORS: true,
      backgroundColor: '#ffffff',
      width: this.receiptWidth,
      windowWidth: this.receiptWidth,
      height: container.scrollHeight,
      windowHeight: container.scrollHeight,
      logging: false
    });
  }

  private createReceiptConfig(printer: string, heightMm: number) {
    return qz.configs.create(printer, {
      units: 'mm',
      margins: 0,
      size: {
        width: this.paperWidthMm,
        height: heightMm,
        custom: true
      },
      density: {
        cross: this.printerDensityDpmm,
        feed: this.printerDensityDpmm
      },
      fallbackDensity: this.printerDensityDpmm,
      orientation: 'portrait',
      scaleContent: false,
      interpolation: 'nearest-neighbor',
      jobName: `Receipt-${Date.now()}`
    });
  }

  private buildReceiptImage(canvas: HTMLCanvasElement) {
    return {
      type: 'pixel',
      format: 'image',
      flavor: 'base64',
      data: canvas.toDataURL('image/png').split(',')[1]
    };
  }

  private calculatePaperHeight(heightPx: number): number {
    const extraFeedMm = 10;
    const minHeightMm = 40;
    const contentHeightMm = heightPx / this.printerDensityDpmm;

    return Math.max(minHeightMm, Number((contentHeightMm + extraFeedMm).toFixed(2)));
  }

  private buildItemsTable(items: ReceiptItemsModel[]): string {
    if (!items.length) {
      return `
        <div style="border:1.5px solid #000;border-radius:10px;padding:14px 10px;text-align:center;font-size:13px;font-weight:700;">
          لا توجد أصناف في الفاتورة
        </div>
      `;
    }

    const headerCellStyle = 'border:1px solid #000;padding:6px 4px;background:#f2f2f2;font-size:12px;font-weight:800;text-align:center;';
    const rowsHtml = items.map((item, index) => {
      const itemTotal = this.getItemTotal(item);
      const priceText = item.price == null ? '-' : this.formatAmount(item.price);
      const rowBackground = index % 2 === 0 ? '#ffffff' : '#fafafa';

      return `
        <tr style="background:${rowBackground};">
          <td style="border:1px solid #000;padding:7px 5px;font-size:12px;font-weight:700;vertical-align:top;word-break:break-word;">
            ${this.escapeHtml(item.name)}
          </td>
          <td style="border:1px solid #000;padding:7px 4px;font-size:12px;text-align:center;vertical-align:middle;">
            ${this.escapeHtml(item.qty)}
          </td>
          <td style="border:1px solid #000;padding:7px 4px;font-size:12px;text-align:center;vertical-align:middle;">
            ${this.escapeHtml(priceText)}
          </td>
          <td style="border:1px solid #000;padding:7px 4px;font-size:12px;font-weight:800;text-align:center;vertical-align:middle;">
            ${this.escapeHtml(this.formatAmount(itemTotal))}
          </td>
        </tr>
      `;
    }).join('');

    return `
      <div style="border:1.5px solid #000;border-radius:10px;overflow:hidden;background:#fff;">
        <table style="width:100%;border-collapse:collapse;table-layout:fixed;font-size:12px;">
          <thead>
            <tr>
              <th style="${headerCellStyle}width:44%;">الصنف</th>
              <th style="${headerCellStyle}width:14%;">الكمية</th>
              <th style="${headerCellStyle}width:21%;">السعر</th>
              <th style="${headerCellStyle}width:21%;">الإجمالي</th>
            </tr>
          </thead>
          <tbody>
            ${rowsHtml}
          </tbody>
        </table>
      </div>
    `;
  }

  private buildInfoRow(label: string, value: unknown): string {
    return `
      <div style="display:flex;justify-content:space-between;align-items:flex-start;gap:10px;padding:4px 2px;border-bottom:1px dashed #d6d6d6;">
        <span style="font-size:12px;font-weight:800;">${this.escapeHtml(label)}</span>
        <span style="font-size:12px;font-weight:600;text-align:left;word-break:break-word;">${this.escapeHtml(value)}</span>
      </div>
    `;
  }

  private buildSummaryRow(label: string, value: string, emphasized = false): string {
    const fontSize = emphasized ? '16px' : '14px';
    const fontWeight = emphasized ? '800' : '700';

    return `
      <div style="display:flex;justify-content:space-between;align-items:center;gap:10px;padding:4px 0;font-size:${fontSize};font-weight:${fontWeight};">
        <span>${this.escapeHtml(label)}</span>
        <span>${this.escapeHtml(value)}</span>
      </div>
    `;
  }

  private getItemsTotal(items: ReceiptItemsModel[]): number {
    return items.reduce((sum, item) => sum + this.getItemTotal(item), 0);
  }

  private getItemTotal(item: ReceiptItemsModel): number {
    const total = Number(item.total);

    if (Number.isFinite(total)) {
      return total;
    }

    return Number(item.qty ?? 0) * Number(item.price ?? 0);
  }

  private showPrintError(error: any): string {
    const message = this.handlePrintError(error);
    this.toaster.error(message, 'خطأ في الطباعة');
    console.error('QZ print error', error);
    return message;
  }

  private blobToBase64(blob: Blob): Promise<string> {
    return new Promise((resolve, reject) => {
      const reader = new FileReader();
      reader.onload = () => resolve((reader.result as string) || '');
      reader.onerror = () => reject(reader.error);
      reader.readAsDataURL(blob);
    });
  }

  private createPrintContainer(result: ReceiptModel): HTMLDivElement {
    const container = document.createElement('div');
    container.style.position = 'fixed';
    container.style.left = '-9999px';
    container.style.top = '0';
    container.style.width = `${this.receiptWidth}px`;
    container.style.background = '#fff';
    container.style.direction = 'rtl';
    container.style.zIndex = '-1';
    container.setAttribute('dir', 'rtl');
    container.innerHTML = this.BuildReceiptBody(result);
    return container;
  }

  private async waitForContainerReady(container: HTMLElement): Promise<void> {
    await this.waitForImages(container);
    await this.waitForNextPaint();
  }

  private waitForImages(container: HTMLElement): Promise<void> {
    const pendingImages = Array.from(container.querySelectorAll('img'))
      .filter(image => !image.complete);

    if (!pendingImages.length) {
      return Promise.resolve();
    }

    return Promise.all(
      pendingImages.map(image => new Promise<void>(resolve => {
        const done = () => resolve();
        image.addEventListener('load', done, { once: true });
        image.addEventListener('error', done, { once: true });
      }))
    ).then(() => undefined);
  }

  private waitForNextPaint(): Promise<void> {
    return new Promise(resolve => {
      requestAnimationFrame(() => requestAnimationFrame(() => resolve()));
    });
  }

  private escapeHtml(value: unknown): string {
    return String(value ?? '')
      .replace(/&/g, '&amp;')
      .replace(/</g, '&lt;')
      .replace(/>/g, '&gt;')
      .replace(/"/g, '&quot;')
      .replace(/'/g, '&#39;');
  }

  private formatAmount(value?: number): string {
    const numericValue = Number(value);

    if (!Number.isFinite(numericValue)) {
      return '-';
    }

    return new Intl.NumberFormat('en-US', {
      minimumFractionDigits: 0,
      maximumFractionDigits: 0
    }).format(numericValue);
  }
}
