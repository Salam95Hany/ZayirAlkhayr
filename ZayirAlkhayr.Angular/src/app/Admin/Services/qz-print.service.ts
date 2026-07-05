import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import html2canvas from 'html2canvas';
import qz from 'qz-tray';
import { ReceiptItemsModel, ReceiptModel } from '../Models/ReceiptModel';

type CropBounds = {
  x: number;
  y: number;
  width: number;
  height: number;
};

type RgbColor = {
  r: number;
  g: number;
  b: number;
};

@Injectable({
  providedIn: 'root'
})
export class QzPrintService {
  private readonly printableWidthMm = 72;
  private readonly printerDensityDpmm = 8;
  private readonly receiptWidth = this.printableWidthMm * this.printerDensityDpmm;
  private readonly logoPath = 'assets/Dams_Star.png';
  private readonly preferredPrinterKeywords = ['xp-q810k', 'xprinter'];
  private readonly storeName = '';
  private readonly storePhones = '44355226 - 44355227';
  private readonly storeAddress = 'الدوحه.المنتزه.شارع حطين';
  private readonly baseFontSizePx = 26;
  private readonly logoDisplayWidthPx = 190;
  private readonly logoOutputPaddingPx = 24;
  private readonly minRenderScale = 4;
  private readonly printBottomPaddingPx = 48;
  private logoBase64 = '';
  private qzSecurityInitialized = false;
  private logoLoadPromise: Promise<void>;
  private qzConnectionPromise?: Promise<void>;

  constructor(private toaster: ToastrService) {
    this.logoLoadPromise = this.loadLogo();
  }

  async Print(result: ReceiptModel): Promise<void> {
    let container: HTMLDivElement | null = null;

    try {
      await this.logoLoadPromise;
      //this.setupQzSecurity();
      await this.InitQZ();

      const printer = await this.resolvePrinterName();
      container = this.createPrintContainer(result);
      document.body.appendChild(container);
      await this.waitForContainerReady(container);
      const canvas = await this.renderReceipt(container);
      const printCanvas = this.prepareCanvasForPrint(canvas);
      const config = this.createReceiptConfig(printer, this.calculatePaperHeight(printCanvas.height));
      await qz.print(config, [this.buildReceiptImage(printCanvas)]);
    } catch (error) {
      const message = this.showPrintError(error);
      const printableError = error instanceof Error ? error : new Error(message);

      (printableError as Error & { userMessage?: string }).userMessage = message;
      throw printableError;
    } finally {
      container?.remove();
    }
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
    const customerHtml = order.orderType == 'توصيل' ? this.buildInfoRow('العميل', order.agent) : '';
    const deliveryFeeHtml = deliveryFee > 0
      ? this.buildSummaryRow('رسوم التوصيل', `${this.formatAmount(deliveryFee)}  QR`)
      : '';

    return `
      <div style="width:${this.receiptWidth}px;background:#fff;color:#000;box-sizing:border-box;">
        <div style="
          width:100%;
          padding:5px 10px;
          box-sizing:border-box;
          font-family:'Tahoma', sans-serif;
          direction:rtl;
          text-align:right;
          line-height:1.3;
          background:#fff;
          font-size:${this.baseFontSizePx}px;
        ">
          <div style="border:2px solid #000;border-radius:16px;padding:0px 10px 5px;background:#fff;">
            ${this.logoBase64 ? `
              <div style="display:flex;justify-content:center;align-items:center;padding:1px 0 0px;">
                <img
                  src="${this.logoBase64}"
                  alt="logo"
                  style="width:${this.logoDisplayWidthPx}px;max-width:100%;height:auto;display:block;object-fit:contain;"
                />
              </div>
            ` : ''}

            <div style="text-align:center;">
              <div style="
                font-size:36px;
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
              <div style="font-size:20px;font-weight:700;">الهاتف: ${this.escapeHtml(this.storePhones)}</div>
              <div style="font-size:20px;font-weight:700;word-break:break-word;">العنوان: ${this.escapeHtml(this.storeAddress)}</div>
            </div>
          </div>

          <div style="margin-top:6px;border:1.5px solid #000;border-radius:12px;padding:12px;background:#fff;">
            <div style="font-size:15px;font-weight:800;text-align:center;background:#f3f3f3;border:2px solid #000;border-radius:9px;padding:4px 8px;margin-bottom:6px;">
              بيانات الطلب
            </div>
            ${this.buildInfoRow('رقم الطلب', order.orderNo)}
            ${this.buildInfoRow('نوع الطلب', order.orderType)}
            ${this.buildInfoRow('التاريخ', order.date)}
            ${this.buildInfoRow('الكاشير', order.cashier)}
            ${customerHtml}
          </div>

          <div style="margin-top:6px;">
            <div style="display:flex;justify-content:space-between;align-items:center;margin-bottom:8px;padding:0 2px;font-size:16px;font-weight:800;">
              <span>تفاصيل الأصناف</span>
              <span>${this.escapeHtml(itemsCount)} صنف</span>
            </div>
            ${this.buildItemsTable(items)}
          </div>

          <div style="margin-top:6px;border:2px solid #000;border-radius:12px;padding:8px 10px;background:#fff;">
            <div style="font-size:15px;font-weight:800;text-align:center;background:#f3f3f3;border:2px solid #000;border-radius:9px;padding:4px 8px;margin-bottom:6px;">
              ملخص الحساب
            </div>
            ${this.buildSummaryRow('إجمالي الأصناف', `${this.formatAmount(itemsTotal)}  QR`)}
            ${deliveryFeeHtml}
            <div style="border-top:2px dashed #000;margin:10px 0 8px;"></div>
            ${this.buildSummaryRow('الإجمالي النهائي', `${this.formatAmount(grandTotal)}  QR`, true)}
          </div>

          <div style="margin-top:6px;border-top:2px dashed #000;padding-top:6px;text-align:center;">
            <div style="font-size:20px;font-weight:700;">شكراً لزيارتكم</div>
            <!-- <div style="font-size:18px;">نتشرف بخدمتكم دائماً</div> -->
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
      this.logoBase64 = await this.createPrintOptimizedLogo(blob);
    } catch (error) {
      this.logoBase64 = '';
      console.error('Logo loading error', error);
    }
  }

  private async createPrintOptimizedLogo(blob: Blob): Promise<string> {
    const sourceBase64 = await this.blobToBase64(blob);

    try {
      const image = await this.loadImageElement(sourceBase64);
      const crop = this.detectLogoBounds(image);
      const workingCanvas = document.createElement('canvas');
      workingCanvas.width = crop.width;
      workingCanvas.height = crop.height;

      const workingContext = workingCanvas.getContext('2d');
      if (!workingContext) {
        return sourceBase64;
      }

      workingContext.drawImage(
        image,
        crop.x,
        crop.y,
        crop.width,
        crop.height,
        0,
        0,
        crop.width,
        crop.height
      );

      const imageData = workingContext.getImageData(0, 0, crop.width, crop.height);
      const { data } = imageData;

      for (let index = 0; index < data.length; index += 4) {
        const alpha = data[index + 3];
        if (alpha === 0) {
          continue;
        }

        const luminance = (data[index] * 0.299) + (data[index + 1] * 0.587) + (data[index + 2] * 0.114);
        // const inverted = 255 - luminance;
        const contrasted = this.clampColor(((luminance - 128) * 1.55) + 128 + 8);
        const tone = contrasted > 242 ? 255 : contrasted < 18 ? 0 : contrasted;

        data[index] = tone;
        data[index + 1] = tone;
        data[index + 2] = tone;
      }

      workingContext.putImageData(imageData, 0, 0);

      const outputCanvas = document.createElement('canvas');
      const padding = this.logoOutputPaddingPx;
      outputCanvas.width = crop.width + (padding * 2);
      outputCanvas.height = crop.height + (padding * 2);

      const outputContext = outputCanvas.getContext('2d');
      if (!outputContext) {
        return sourceBase64;
      }

      outputContext.fillStyle = '#ffffff';
      outputContext.fillRect(0, 0, outputCanvas.width, outputCanvas.height);
      outputContext.imageSmoothingEnabled = true;
      outputContext.imageSmoothingQuality = 'high';
      outputContext.drawImage(workingCanvas, padding, padding);

      return outputCanvas.toDataURL('image/png');
    } catch (error) {
      console.error('Logo processing error', error);
      return sourceBase64;
    }
  }

  private loadImageElement(source: string): Promise<HTMLImageElement> {
    return new Promise((resolve, reject) => {
      const image = new Image();
      image.onload = () => resolve(image);
      image.onerror = () => reject(new Error('Failed to process logo image.'));
      image.src = source;
    });
  }

  private detectLogoBounds(image: HTMLImageElement): CropBounds {
    const width = image.naturalWidth || image.width;
    const height = image.naturalHeight || image.height;
    const fullBounds: CropBounds = { x: 0, y: 0, width, height };
    const canvas = document.createElement('canvas');
    canvas.width = width;
    canvas.height = height;

    const context = canvas.getContext('2d');
    if (!context) {
      return fullBounds;
    }

    context.drawImage(image, 0, 0, width, height);
    const imageData = context.getImageData(0, 0, width, height);
    const backgroundColor = this.getAverageCornerColor(imageData);
    const { data } = imageData;
    const step = Math.max(1, Math.floor(Math.min(width, height) / 400));
    let minX = width;
    let minY = height;
    let maxX = -1;
    let maxY = -1;

    for (let y = 0; y < height; y += step) {
      for (let x = 0; x < width; x += step) {
        const index = ((y * width) + x) * 4;
        const alpha = data[index + 3];
        if (alpha < 8) {
          continue;
        }

        const distance = this.getColorDistance(
          data[index],
          data[index + 1],
          data[index + 2],
          backgroundColor.r,
          backgroundColor.g,
          backgroundColor.b
        );
        const luminance = (data[index] * 0.299) + (data[index + 1] * 0.587) + (data[index + 2] * 0.114);

        if (distance < 30 && luminance < 80) {
          continue;
        }

        minX = Math.min(minX, x);
        minY = Math.min(minY, y);
        maxX = Math.max(maxX, x);
        maxY = Math.max(maxY, y);
      }
    }

    if (maxX < minX || maxY < minY) {
      return fullBounds;
    }

    const padding = Math.max(18, Math.floor(Math.min(width, height) * 0.03));
    const cropX = Math.max(0, minX - padding);
    const cropY = Math.max(0, minY - padding);
    const cropRight = Math.min(width, maxX + padding + step);
    const cropBottom = Math.min(height, maxY + padding + step);

    return {
      x: cropX,
      y: cropY,
      width: Math.max(1, cropRight - cropX),
      height: Math.max(1, cropBottom - cropY)
    };
  }

  private getAverageCornerColor(imageData: ImageData): RgbColor {
    const { width, height, data } = imageData;
    const sampleSize = Math.max(12, Math.floor(Math.min(width, height) * 0.05));
    const corners = [
      { x: 0, y: 0 },
      { x: Math.max(0, width - sampleSize), y: 0 },
      { x: 0, y: Math.max(0, height - sampleSize) },
      { x: Math.max(0, width - sampleSize), y: Math.max(0, height - sampleSize) }
    ];

    let totalR = 0;
    let totalG = 0;
    let totalB = 0;
    let totalCount = 0;

    corners.forEach(corner => {
      for (let y = corner.y; y < corner.y + sampleSize && y < height; y++) {
        for (let x = corner.x; x < corner.x + sampleSize && x < width; x++) {
          const index = ((y * width) + x) * 4;
          totalR += data[index];
          totalG += data[index + 1];
          totalB += data[index + 2];
          totalCount++;
        }
      }
    });

    if (!totalCount) {
      return { r: 0, g: 0, b: 0 };
    }

    return {
      r: Math.round(totalR / totalCount),
      g: Math.round(totalG / totalCount),
      b: Math.round(totalB / totalCount)
    };
  }

  private getColorDistance(r1: number, g1: number, b1: number, r2: number, g2: number, b2: number): number {
    const deltaR = r1 - r2;
    const deltaG = g1 - g2;
    const deltaB = b1 - b2;

    return Math.sqrt((deltaR * deltaR) + (deltaG * deltaG) + (deltaB * deltaB));
  }

  private clampColor(value: number): number {
    return Math.max(0, Math.min(255, Math.round(value)));
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
      scale: this.getRenderScale(),
      useCORS: true,
      backgroundColor: '#ffffff',
      height: container.scrollHeight,
      windowHeight: container.scrollHeight,
      imageTimeout: 0,
      logging: false
    });
  }

  private getRenderScale(): number {
    const deviceScale = typeof window !== 'undefined' ? (window.devicePixelRatio || 1) : 1;
    return Math.min(4, Math.max(this.minRenderScale, Math.ceil(deviceScale)));
  }

  private prepareCanvasForPrint(canvas: HTMLCanvasElement): HTMLCanvasElement {
    if (canvas.width === this.receiptWidth) {
      return canvas;
    }

    const resizedHeight = Math.max(1, Math.ceil(canvas.height * (this.receiptWidth / canvas.width)));
    const outputCanvas = document.createElement('canvas');
    outputCanvas.width = this.receiptWidth;
    outputCanvas.height = resizedHeight + this.printBottomPaddingPx;

    const context = outputCanvas.getContext('2d');
    if (!context) {
      return canvas;
    }

    context.fillStyle = '#ffffff';
    context.fillRect(0, 0, outputCanvas.width, outputCanvas.height);
    context.imageSmoothingEnabled = true;
    context.imageSmoothingQuality = 'high';
    context.drawImage(canvas, 0, 0, outputCanvas.width, resizedHeight);

    return outputCanvas;
  }

  private createReceiptConfig(printer: string, heightMm: number) {
    return qz.configs.create(printer, {
      units: 'mm',
      margins: 0,
      size: {
        width: this.printableWidthMm,
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
      interpolation: 'bicubic',
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
    const extraFeedMm = 12;
    const minHeightMm = 40;
    const contentHeightMm = heightPx / this.printerDensityDpmm;

    return Math.max(minHeightMm, Number((Math.ceil(contentHeightMm) + extraFeedMm).toFixed(2)));
  }

  private buildItemsTable(items: ReceiptItemsModel[]): string {
    if (!items.length) {
      return `
        <div style="border:1.5px solid #000;border-radius:10px;padding:16px 12px;text-align:center;font-size:20px;font-weight:700;">
          لا توجد أصناف في الفاتورة
        </div>
      `;
    }

    const headerCellStyle = 'border:2px solid #000;padding:8px 5px;background:#f2f2f2;font-size:18px;font-weight:800;text-align:center;';
    const rowsHtml = items.map((item, index) => {
      const itemTotal = this.getItemTotal(item);
      const priceText = item.price == null ? '-' : this.formatAmount(item.price);
      const rowBackground = index % 2 === 0 ? '#ffffff' : '#fafafa';

      return `
        <tr style="background:${rowBackground};">
          <td style="border:2px solid #000;padding:5px 4px;font-size:20px;font-weight:700;vertical-align:top;word-break:break-word;">
            ${this.escapeHtml(item.nameAr)}
          </td>
          <td style="border:2px solid #000;padding:5px 4px;font-size:20px;font-weight:700;vertical-align:top;word-break:break-word;">
            ${this.escapeHtml(item.nameEn)}
          </td>
          <td style="border:2px solid #000;padding:8px 5px;font-size:18px;text-align:center;vertical-align:middle;">
            ${this.escapeHtml(item.qty)}
          </td>
          <td style="border:2px solid #000;padding:8px 5px;font-size:18px;text-align:center;vertical-align:middle;">
            ${this.escapeHtml(priceText)}
          </td>
          <td style="border:2px solid #000;padding:8px 5px;font-size:18px;font-weight:800;text-align:center;vertical-align:middle;">
            ${this.escapeHtml(this.formatAmount(itemTotal))}  QR
          </td>
        </tr>
      `;
    }).join('');

    return `
      <div style="border:1.5px solid #000;border-radius:10px;overflow:hidden;background:#fff;">
        <table style="width:100%;border-collapse:collapse;table-layout:fixed;font-size:18px;">
          <thead>
            <tr>
              <th style="${headerCellStyle}width:44%;">الصنف</th>
              <th style="${headerCellStyle}width:44%;">Item</th>
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
      <div style="display:flex;justify-content:space-between;align-items:flex-start;gap:10px;padding:3px 2px;border-bottom:1px dashed #d6d6d6;">
        <span style="font-size:20px;font-weight:800;">${this.escapeHtml(label)}</span>
        <span style="font-size:20px;font-weight:700;text-align:left;word-break:break-word;">${this.escapeHtml(value)}</span>
      </div>
    `;
  }

  private buildSummaryRow(label: string, value: string, emphasized = false): string {
    const fontSize = emphasized ? '20px' : '18px';
    const fontWeight = emphasized ? '800' : '700';

    return `
      <div style="display:flex;justify-content:space-between;align-items:center;gap:10px;padding:5px 0;font-size:${fontSize};font-weight:${fontWeight};">
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
    container.style.maxWidth = '100%';
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

  private setupQzSecurity(): void {
    if (this.qzSecurityInitialized) {
      return;
    }

    qz.security.setCertificatePromise((resolve, reject) => {
      fetch('/assets/qz/digital-certificate.txt', { cache: 'no-store' })
        .then(res => {
          if (!res.ok) {
            throw new Error(`Failed to load certificate: ${res.status} ${res.statusText}`);
          }
          return res.text();
        })
        .then(resolve)
        .catch(reject);
    });

    qz.security.setSignatureAlgorithm('SHA512');

    qz.security.setSignaturePromise((toSign) => {
      return (resolve, reject) => {
        fetch('http://127.0.0.1:3000/sign', {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify({ data: toSign })
        })
          .then(res => {
            if (!res.ok) {
              throw new Error(`Signing server error: ${res.status}`);
            }
            return res.text();
          })
          .then(resolve)
          .catch(reject);
      };
    });

    this.qzSecurityInitialized = true;
  }
}
