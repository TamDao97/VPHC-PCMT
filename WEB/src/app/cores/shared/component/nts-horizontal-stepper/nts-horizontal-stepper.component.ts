import { Component, OnInit } from '@angular/core';
import { CdkStepper } from '@angular/cdk/stepper';

@Component({
  selector: 'app-nts-horizontal-stepper',
  templateUrl: './nts-horizontal-stepper.component.html',
  styleUrls: ['./nts-horizontal-stepper.component.scss'],
  providers: [{ provide: CdkStepper, useExisting: NtsHorizontalStepperComponent }]
})
export class NtsHorizontalStepperComponent extends CdkStepper implements OnInit {

  ngOnInit(): void {
  }

  onClick(index: number): void {
    this.selectedIndex = index;
  }
}
