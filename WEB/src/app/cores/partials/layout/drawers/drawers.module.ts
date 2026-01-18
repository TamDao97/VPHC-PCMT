import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { InlineSVGModule } from 'ng-inline-svg-2';
import { ActivityDrawerComponent } from './activity-drawer/activity-drawer.component';
import { MessengerDrawerComponent } from './messenger-drawer/messenger-drawer.component';
import { ChatInnerModule } from '../../content/chat-inner/chat-inner.module';
import { SharedModule } from "../../../shared/shared.module";
import {  NtsSearchDrawerComponent } from './nts-search-drawer/nts-search-drawer.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { DropDownListModule } from '@syncfusion/ej2-angular-dropdowns';
import { DatePickerModule } from '@syncfusion/ej2-angular-calendars';

@NgModule({
  declarations: [
    ActivityDrawerComponent,
    MessengerDrawerComponent,
    NtsSearchDrawerComponent
  ],
  imports: [CommonModule, InlineSVGModule, RouterModule, ChatInnerModule, SharedModule,NgbModule,FormsModule ,DropDownListModule,DatePickerModule,ReactiveFormsModule ],
  exports: [
    ActivityDrawerComponent,
    MessengerDrawerComponent,
    NtsSearchDrawerComponent,
  ],
})
export class DrawersModule {}
