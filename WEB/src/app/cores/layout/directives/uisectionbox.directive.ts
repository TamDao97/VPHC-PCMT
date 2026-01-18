import { Directive, HostListener, OnDestroy } from '@angular/core';

declare var $: any;

@Directive({
  selector: '[appUisectionbox]'
})
export class UisectionboxDirective implements OnDestroy {

  constructor() {
    $(document).off('click', '.box header .actions i.box_toggle');
    $(document).off('click', '.box header .actions i.box_close');
    $(document).on('click', '.box header .actions i.box_toggle', function(e:any) {
      var _this = $(e.currentTarget);    

      if (_this.hasClass("fa-chevron-down")) {
        _this.toggleClass("fa-chevron-down") && _this.toggleClass("fa-chevron-up") && e.preventDefault();
      } else if (_this.hasClass("fa-chevron-up")) {
        _this.toggleClass("fa-chevron-up") && _this.toggleClass("fa-chevron-down") && e.preventDefault();
      }

      _this.parent().parent().parent().toggleClass("collapsed");
    });

    $(document).on('click', '.box header .actions i.box_close', function(e:any) {
      var _this = $(e.currentTarget);
      _this.parent().parent().parent().addClass("hide").hide() && e.preventDefault();
    });
  }

  ngOnDestroy(): void {
    $(document).off('click', '.box header .actions i.box_toggle', function(e:any){});
    $(document).off('click', '.box header .actions i.box_close', function(e:any){});
  }
}
