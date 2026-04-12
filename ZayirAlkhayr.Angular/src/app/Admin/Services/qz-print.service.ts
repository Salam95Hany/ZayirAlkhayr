import { Injectable } from '@angular/core';
import qz from 'qz-tray';
import html2canvas from 'html2canvas';
import { ReceiptItemsModel, ReceiptModel } from '../Models/ReceiptModel';

@Injectable({
  providedIn: 'root'
})
export class QzPrintService {
  private logoBase64: string = '';
  constructor() {
    this.InitQZ();
    this.loadLogo();
  }

  loadLogo(): void {
    fetch('../../../assets/PosLogo.jpeg')
      .then(response => response.blob())
      .then(blob => {
        const reader = new FileReader();
        reader.onload = () => {
          this.logoBase64 = reader.result as string;
        };
        reader.readAsDataURL(blob);
      });
  }

  InitQZ() {
    qz.websocket.connect({ usingSecure: true })
      .then(() => console.log('QZ Connected Securely'))
      .catch(err => console.error('QZ Connection Error', err));
  }

  GetPrinters() { return qz.printers.find(); }
  GetDefaultPrinter() { return qz.printers.getDefault(); }

  Print(result: ReceiptModel): Promise<void> {
  return new Promise((resolve, reject) => {
    this.GetDefaultPrinter().then(printer => {
      const config = qz.configs.create(printer);
      const container = document.createElement('div');

      container.style.position = 'fixed';
      container.style.left = '-9999px';
      container.style.width = '384px';
      container.style.direction = 'rtl';
      container.innerHTML = this.BuildReceiptBody(result);
      document.body.appendChild(container);

      setTimeout(() => {
        html2canvas(container, {
          scale: 2,
          useCORS: true,
          backgroundColor: '#fff',
          width: 384,
          windowWidth: 384
        }).then(canvas => {

          const imageData = canvas.toDataURL('image/png').split(',')[1];

          qz.print(config, [{
            type: 'pixel',
            format: 'image',
            flavor: 'base64',
            data: imageData
          }])
          .then(() => resolve())
          .catch(err => reject(err))
          .finally(() => {
            document.body.removeChild(container);
          });
        }).catch(err => {
          document.body.removeChild(container);
          reject(err);
        });
      }, 300);
    }).catch(err => reject(err));
  });
}

  BuildReceiptBody(order: ReceiptModel): string {
    let itemsHtml = '';
    order.items.forEach(item => {
      itemsHtml += `
        <tr style="border:1px solid #000;text-align:right;font-weight:bold;">
          <td style="border:1px solid #000;">${item.name}</td>
          <td style="border:1px solid #000;">${item.qty}</td>
          <td style="border:1px solid #000;">${item.price}</td>
          <td style="border:1px solid #000;">${item.total}</td>
        </tr>
      `;
    });

    return `
      <div style="
        width:384px;
        font-size:13px;
        font-family: Tahoma, Arial, sans-serif;
        direction: rtl;
        text-align: right;
        color: #000;
        background: #fff;
      ">
        <div style="display:flex;flex-direction:column;justify-content:center;align-items:center;text-align:center;margin-top: 20px;">
        <img 
          src="${this.logoBase64}" 
          style="width:50px; height:50px; object-fit:contain;"
          crossorigin="anonymous"
        />
        <h2 style="margin:0; display:flex; align-items:flex-end; justify-content:center; gap:8px; line-height:1;">
		  <span style="transform: translateY(5px);">***</span>
		  <span>صبح و مسا</span>
		  <span style="transform: translateY(5px);">***</span>
		</h2>
      </div>
        <hr/>
        <h4 style="margin:2px;">الهاتف: 0998222283 - 0998222286</h4>
        <h4 style="margin:2px;">العنوان: درعا جاسم شرق المركز الثقافي 200 م</h4>
        <hr style="border:none;height:3px;background:#000;"/>
        <h4 style="margin:2px;">الكاشير: ${order.cashier}</h4>
        <h4 style="margin:2px;">رقم التسلسلي: ${order.orderNo}</h4>
        <h4 style="margin:2px;">التاريخ: ${order.date}</h4>
        <h4 style="margin:2px;">نوع الطلب: ${order.orderType}</h4>
        <hr style="border:none;height:3px;background:#000;"/>
        ${order.agent ? `<h4 style="margin:2px;">العميل: ${order.agent}</h4>` : ''}
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
            ${order.agent ? `
            <tr>
              <td colspan="3" style="border:1px solid #000;text-align:right;font-weight:bold;">رسوم التوصيل</td>
              <td style="border:1px solid #000;text-align:right;font-weight:bold;">10000ل.س</td>
            </tr>` : ''}
            <tr>
              <td colspan="3" style="border:1px solid #000;text-align:right;font-weight:bold;">المجموع النهائي</td>
              <td style="border:1px solid #000;text-align:right;font-weight:bold;">${order.grandTotal}ل.س</td>
            </tr>
          </tbody>
        </table>
      </div>
    `;
  }

  GroupItemsByCategory(items: ReceiptItemsModel[]) {
    const grouped: { [key: string]: any[] } = {};
    items.forEach(item => {
      if (!grouped[item.categoryId]) grouped[item.categoryId] = [];
      grouped[item.categoryId].push(item);
    });
    return grouped;
  }
}
