import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'enumText'
})
export class EnumTextPipe implements PipeTransform {

  transform(value: number, type: 'orderType' | 'orderStatus'): string {
    if (type === 'orderType') {
      switch (value) {
        case 1: return 'خارجي';
        case 2: return 'توصيل';
        default: return '';
      }
    }

    if (type === 'orderStatus') {
      switch (value) {
        case 1: return 'منتهي';
        case 2: return 'ملغي';
        default: return '';
      }
    }

    return '-';
  }
}
