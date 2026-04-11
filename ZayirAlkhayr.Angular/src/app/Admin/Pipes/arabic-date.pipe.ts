import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'arabicDate'
})
export class ArabicDatePipe implements PipeTransform {

  transform(value: string | Date): string {
    if (!value) return '';

    const date = new Date(value);

    return date.toLocaleDateString('ar-EG', {
      day: 'numeric',
      month: 'long',
      year: 'numeric'
    });
  }
}
