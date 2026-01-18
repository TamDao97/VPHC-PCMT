import { Component, Input, AfterViewInit, ViewChild, ElementRef } from '@angular/core';

@Component({
  selector: 'app-nts-text-more',
  templateUrl: './nts-text-more.component.html',
  styleUrls: ['./nts-text-more.component.scss']
})
export class NtsTextMoreComponent implements AfterViewInit {
  @Input() ntsText: string = '';      // Văn bản truyền vào
  @Input() ntsLimit: number = 2;   // Số dòng tối đa
  @ViewChild('textContainer') textContainer!: ElementRef;

  isExpanded: boolean = false;     // Trạng thái mở rộng/thu gọn
  isOverflowing: boolean = false;  // Kiểm tra văn bản có vượt quá không

  ngAfterViewInit() {
    // Trì hoãn kiểm tra để đảm bảo DOM đã được render hoàn chỉnh
    setTimeout(() => {
      this.checkTextOverflow();
    });
  }

  checkTextOverflow() {
    const el = this.textContainer.nativeElement;
    const lineHeight = parseFloat(getComputedStyle(el).lineHeight); // Chiều cao 1 dòng
    const maxHeight = this.ntsLimit * lineHeight; // Tổng chiều cao cho maxLines dòng

    // So sánh chiều cao thật của phần tử với chiều cao tối đa
    this.isOverflowing = el.scrollHeight > maxHeight;
  }

  toggleText() {
    this.isExpanded = !this.isExpanded;
  }
}

