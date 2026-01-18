import { Injectable } from '@angular/core';
import {
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class DateInterceptor implements HttpInterceptor {
  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    if (req.body && !(req.body instanceof FormData) && typeof req.body === 'object') {
      const modifiedBody = this.convertDates(req.body);

      // Clone request và thay đổi body
      const clonedRequest = req.clone({
        body: modifiedBody,
      });

      return next.handle(clonedRequest);
    }
    return next.handle(req);
  }

  private convertDates(body: any, timeZoneOffset: number = 7): any {
    if (body === null || body === undefined || typeof body !== 'object') {
      return body;
    }
  
    const newBody: Record<string, any> = Array.isArray(body) ? [] : {};
  
    for (const key of Object.keys(body)) {
      const value = body[key];
      if (value instanceof Date) {
        // Chuyển đổi Date sang múi giờ chỉ định (UTC + Offset)
        const adjustedDate = new Date(value.getTime() + timeZoneOffset * 60 * 60 * 1000); // Điều chỉnh múi giờ
        const isoString = adjustedDate.toISOString();
        newBody[key] = isoString.replace('Z', `+${String(timeZoneOffset).padStart(2, '0')}:00`); // Thay Z bằng múi giờ +07:00
      } else if (typeof value === 'object') {
        // Xử lý đệ quy cho các object con
        newBody[key] = this.convertDates(value, timeZoneOffset);
      } else {
        // Nếu không phải Date hoặc object, giữ nguyên giá trị
        newBody[key] = value;
      }
    }
  
    return newBody;
  }
}
