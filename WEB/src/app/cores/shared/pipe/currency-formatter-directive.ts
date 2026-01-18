import { Directive, ElementRef, HostListener, Input, forwardRef } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';

@Directive({
  selector: '[appCurrencyFormatter]',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => CurrencyFormatterDirective),
      multi: true
    }
  ]
})
export class CurrencyFormatterDirective implements ControlValueAccessor {
  private previousValue: string = ''; // Lưu giá trị trước đó

  constructor(private el: ElementRef) {}

  // Các phương thức trong ControlValueAccessor để đồng bộ giá trị

  writeValue(value: any): void {
    if (value !== undefined && value !== null) {
      this.el.nativeElement.value = this.format(value);
    }
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  setDisabledState?(isDisabled: boolean): void {
    this.el.nativeElement.disabled = isDisabled;
  }

  private onChange: any = () => {};
  private onTouched: any = () => {};

  @HostListener('input', ['$event.target.value'])
  onInput(value: string): void {
    const numericValue = this.unformat(value); // Loại bỏ ký tự định dạng

    if (isNaN(Number(numericValue))) {
      // Nếu giá trị không hợp lệ, phục hồi giá trị trước đó
      this.el.nativeElement.value = this.previousValue;
    } else {
      // Cập nhật giá trị trong model
      this.onChange(numericValue);

      // Định dạng giá trị và cập nhật trong ô input
      const formattedValue = this.format(numericValue);
      this.el.nativeElement.value = formattedValue;

      // Cập nhật giá trị trước đó
      this.previousValue = formattedValue;
    }
  }

  private format(value: string | number): string {
    if (!value) return '';
    const number = parseFloat(value.toString());
    return number.toLocaleString('vi-VN'); // Định dạng theo kiểu Việt Nam
  }

  private unformat(value: string): string {
    return value.replace(/[^\d]/g, ''); // Loại bỏ tất cả ký tự không phải số
  }
}
