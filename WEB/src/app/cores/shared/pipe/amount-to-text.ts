import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'amountToText'
})
export class AmountToTextPipe implements PipeTransform {
  private units: string[] = ['', 'nghìn', 'triệu', 'tỷ', 'nghìn tỷ', 'triệu tỷ'];
  private numbers: string[] = [
    'không', 'một', 'hai', 'ba', 'bốn',
    'năm', 'sáu', 'bảy', 'tám', 'chín'
  ];

  transform(value: number, currency: string = 'đồng'): string {
    if(!value && value !== 0){
      return '';
    } 
    if (!value || value === 0) {
      return `Không ${currency}`;
    }

    const parts = [];
    let numberStr = value.toString();

    // Chia số thành từng nhóm 3 chữ số (theo thứ tự từ phải sang trái)
    while (numberStr.length > 3) {
      parts.unshift(numberStr.slice(-3));
      numberStr = numberStr.slice(0, -3);
    }
    parts.unshift(numberStr);

    let text = '';
    for (let i = 0; i < parts.length; i++) {
      const groupText = this.readThreeDigits(parseInt(parts[i], 10));
      if (groupText) {
        text += `${groupText} ${this.units[parts.length - 1 - i]} `;
      }
    }

    // Viết hoa chữ cái đầu tiên
    text = `${text.trim()} ${currency}`.replace(/\s+/g, ' ');
    return text.charAt(0).toUpperCase() + text.slice(1);
  }

  private readThreeDigits(number: number): string {
    const hundred = Math.floor(number / 100);
    const ten = Math.floor((number % 100) / 10);
    const unit = number % 10;
    let result = '';

    if (hundred > 0) {
      result += `${this.numbers[hundred]} trăm `;
      if (ten === 0 && unit > 0) {
        result += 'lẻ ';
      }
    }

    if (ten > 1) {
      result += `${this.numbers[ten]} mươi `;
      if (unit === 1) {
        result += 'mốt ';
      } else if (unit === 5) {
        result += 'lăm ';
      } else if (unit > 0) {
        result += `${this.numbers[unit]} `;
      }
    } else if (ten === 1) {
      result += 'mười ';
      if (unit === 5) {
        result += 'lăm ';
      } else if (unit > 0) {
        result += `${this.numbers[unit]} `;
      }
    } else if (unit > 0) {
      result += `${this.numbers[unit]} `;
    }

    return result.trim();
  }
}
