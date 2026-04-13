import { Injectable } from '@angular/core';
import qz from 'qz-tray';
import html2canvas from 'html2canvas';
import { ReceiptItemsModel, ReceiptModel } from '../Models/ReceiptModel';

@Injectable({
  providedIn: 'root'
})
export class QzPrintService {
  private readonly receiptWidth = 384;
  private readonly logoPath = 'assets/PosLogo.jpeg';
  private logoBase64 = '';
  private logoLoadPromise: Promise<void>;
  private qzConnectionPromise?: Promise<void>;

  constructor() {
    this.logoLoadPromise = this.loadLogo();
    this.InitQZ().catch(err => console.error('QZ Connection Error', err));
  }

  async loadLogo(): Promise<void> {
    try {
      const response = await fetch(this.logoPath);
      if (!response.ok) {
        throw new Error(`Failed to load logo: ${response.status} ${response.statusText}`);
      }

      const blob = await response.blob();
      this.logoBase64 = await this.blobToBase64(blob);
    } catch (err) {
      this.logoBase64 = '';
      console.error('Logo loading error', err);
    }
  }

  InitQZ(): Promise<void> {
    if (qz.websocket.isActive()) {
      return Promise.resolve();
    }

    if (!this.qzConnectionPromise) {
      this.qzConnectionPromise = qz.websocket.connect({ usingSecure: true })
        .then(() => console.log('QZ Connected Securely'))
        .catch(err => {
          this.qzConnectionPromise = undefined;
          throw err;
        });
    }

    return this.qzConnectionPromise;
  }

  GetPrinters() {
    return this.InitQZ().then(() => qz.printers.find());
  }

  GetDefaultPrinter() {
    return this.InitQZ().then(() => qz.printers.getDefault());
  }

  async Print(result: ReceiptModel): Promise<void> {
    await this.logoLoadPromise;

    const printer = await this.GetDefaultPrinter();
    if (!printer) {
      throw new Error('No default printer was found in QZ Tray.');
    }

    const config = qz.configs.create(printer);
    const container = this.createPrintContainer(result);
    document.body.appendChild(container);

    try {
      await this.waitForContainerReady(container);

      const canvas = await html2canvas(container, {
        scale: 2,
        useCORS: true,
        backgroundColor: '#fff',
        width: this.receiptWidth,
        windowWidth: this.receiptWidth
      });

      const imageData = canvas.toDataURL('image/png').split(',')[1];

      await qz.print(config, [{
        type: 'pixel',
        format: 'image',
        flavor: 'base64',
        data: imageData
      }]);
    } finally {
      container.remove();
    }
  }

  BuildReceiptBody(order: ReceiptModel): string {
    const itemsHtml = (order.items ?? []).map(item => `
      <tr style="border:1px solid #000;text-align:right;font-weight:bold;">
        <td style="border:1px solid #000;word-break:break-word;">${this.escapeHtml(item.name)}</td>
        <td style="border:1px solid #000;">${this.escapeHtml(item.qty)}</td>
        <td style="border:1px solid #000;">${this.escapeHtml(this.formatAmount(item.price))}</td>
        <td style="border:1px solid #000;">${this.escapeHtml(this.formatAmount(item.total))}</td>
      </tr>
    `).join('');

    const customerHtml = order.agent
      ? `<h4 style="margin:2px;">العميل: ${this.escapeHtml(order.agent)}</h4>`
      : '';

    const deliveryFee = Number(order.deliveryFee ?? 0);
    const deliveryFeeHtml = deliveryFee > 0
      ? `
        <tr>
          <td colspan="3" style="border:1px solid #000;text-align:right;font-weight:bold;">رسوم التوصيل</td>
          <td style="border:1px solid #000;text-align:right;font-weight:bold;">${this.escapeHtml(this.formatAmount(deliveryFee))}ل.س</td>
        </tr>
      `
      : '';

    return `
      <div style="
        width:${this.receiptWidth}px;
        display:flex;
        justify-content:center;
        background:#fff;
        color:#000;
      ">
        <div style="
          width:100%;
          margin:0 auto;
          padding:0 12px 8px;
          box-sizing:border-box;
          font-size:13px;
          font-family:Tahoma, Arial, sans-serif;
          direction:rtl;
          text-align:right;
          background:#fff;
        ">
        <div style="display:flex;flex-direction:column;justify-content:center;align-items:center;text-align:center;margin-top:20px;">
          ${this.logoBase64 ? `
            <img
              src="${this.logoBase64}"
              style="width:50px;height:50px;object-fit:contain;"
              crossorigin="anonymous"
            />
          ` : ''}
          <h2 style="margin:0;display:flex;align-items:flex-end;justify-content:center;gap:8px;line-height:1;">
            <span style="transform:translateY(5px);">***</span>
            <span>صبح و مسا</span>
            <span style="transform:translateY(5px);">***</span>
          </h2>
        </div>
        <hr/>
        <h4 style="margin:2px;">الهاتف: 0998222283 - 0998222286</h4>
        <h4 style="margin:2px;">العنوان: درعا جاسم شرق المركز الثقافي 200 م</h4>
        <hr style="border:none;height:3px;background:#000;"/>
        <h4 style="margin:2px;">الكاشير: ${this.escapeHtml(order.cashier)}</h4>
        <h4 style="margin:2px;">رقم التسلسلي: ${this.escapeHtml(order.orderNo)}</h4>
        <h4 style="margin:2px;">التاريخ: ${this.escapeHtml(order.date)}</h4>
        <h4 style="margin:2px;">نوع الطلب: ${this.escapeHtml(order.orderType)}</h4>
        <hr style="border:none;height:3px;background:#000;"/>
        ${customerHtml}
        <table style="width:100%;border-collapse:collapse;border:1px solid #000;">
          <thead>
            <tr>
              <th style="border:1px solid #000;text-align:right;">اسم الصنف</th>
              <th style="border:1px solid #000;text-align:right;">الكمية</th>
              <th style="border:1px solid #000;text-align:right;">السعر</th>
              <th style="border:1px solid #000;text-align:right;">الإجمالي</th>
            </tr>
          </thead>
          <tbody>
            ${itemsHtml}
            ${deliveryFeeHtml}
            <tr>
              <td colspan="3" style="border:1px solid #000;text-align:right;font-weight:bold;">المجموع النهائي</td>
              <td style="border:1px solid #000;text-align:right;font-weight:bold;">${this.escapeHtml(this.formatAmount(order.grandTotal))}ل.س</td>
            </tr>
          </tbody>
        </table>
      </div>
    `;
  }

  GroupItemsByCategory(items: ReceiptItemsModel[]) {
    const grouped: { [key: string]: ReceiptItemsModel[] } = {};
    items.forEach(item => {
      if (!grouped[item.categoryId]) {
        grouped[item.categoryId] = [];
      }

      grouped[item.categoryId].push(item);
    });
    return grouped;
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
    container.style.direction = 'rtl';
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
    return value == null ? '-' : `${value}`;
  }
}
