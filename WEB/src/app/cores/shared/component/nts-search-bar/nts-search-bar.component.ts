import { Component, ViewEncapsulation, OnInit, Input, Output, EventEmitter, forwardRef, ChangeDetectorRef } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { Constants } from '../../common/constants';
import { DateUtils } from '../../common/date-utils';
import { SearchOptions } from '../../models/search-options.model';
@Component({
  selector: 'app-nts-search-bar',
  templateUrl: './nts-search-bar.component.html',
  styleUrls: ['./nts-search-bar.component.scss'],
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => NTSSearchBarComponent),
    multi: true
  }
  ],
  encapsulation: ViewEncapsulation.None
})
export class NTSSearchBarComponent implements ControlValueAccessor {

  constructor(private _cd: ChangeDetectorRef,
    public constants: Constants,
    private dateUtils: DateUtils,) { }

  count = 0;
  @Input()
  get options() { return this._options };
  set options(value: any) {
    this._options = value;
    this.initOptions();
  };

  private _onChange = (_: any) => { };
  private _onTouched = () => { };

  public _options: SearchOptions = new SearchOptions();
  public _searchModel: any = {};
  public _searchModelView: any = {};
  private _searchItemId = 0;
  public _searchItems: any[] = [];
  public _searchValues: any[] = [];

  @Output() changeEvent = new EventEmitter();
  @Output() refreshEvent = new EventEmitter();
  disabled = false;

  ngOnInit() {

  }

  initOptions() {
    this._searchItems = [];
    for (let i = 0; i < this._options.Items.length; i++) {
      this._options.Items[i].Index = i;
      this._searchItems.push({
        Id: i,
        Name: this._options.Items[i].Name,
        Value: null,
        FieldName: this._options.Items[i].FieldName,
        FieldNameTo: this._options.Items[i].FieldNameTo,
        FieldNameFrom: this._options.Items[i].FieldNameFrom,
        Type: this._options.Items[i].Type,
        Permission: this._options.Items[i].Permission,
        RelationIndexTo: this._options.Items[i].RelationIndexTo,
        IsRelation: this._options.Items[i].IsRelation,
        Index: this._options.Items[i].Index,
        Hidden: this._options.Items[i].Hidden,
      });

      if (this._options.Items[i].DataType) {
        // this.getDataByDataType(this._options.Items[i], true);
      }
    }
  }

  writeValue(value: any | any[]): void {
    if (value) {
      this._searchModel = value;
      this._searchModelView = Object.assign({}, value);
      this._searchValues = [];

      for (let i = 0; i < this._searchItems.length; i++) {
        this._searchItems[i].Checked = false;
        this._searchItems[i].Value = null;
        if (!this._options.Items[i].DataType) {
          if (this._searchItems[i].Type != 'date' && this._searchItems[i].Type != 'dateRange') {
            if (this._searchModel[this._searchItems[i].FieldName] != null
              && this._searchModel[this._searchItems[i].FieldName] != ''
              && this._searchModel[this._searchItems[i].FieldName] != undefined) {


              if (this._options.Items[i].Type == 'select' || this._options.Items[i].Type == 'ngselect') {
                this.setValueDefaut(this._options.Items[i]);
              } else {
                this._searchItems[i].Checked = true;
                this._searchItems[i].Value = this._searchModel[this._searchItems[i].FieldName];
              }
            }
          }
          else {
            if (this._searchModel[this._searchItems[i].FieldNameFrom] != null
              && this._searchModel[this._searchItems[i].FieldNameFrom] != ''
              && this._searchModel[this._searchItems[i].FieldNameFrom] != undefined) {
              this._searchItems[i].Checked = true;
              this._searchItems[i].Value = (this._searchModel[this._searchItems[i].FieldNameFrom].day < 10 ? '0' + this._searchModel[this._searchItems[i].FieldNameFrom].day : this._searchModel[this._searchItems[i].FieldNameFrom].day) + '/' + (this._searchModel[this._searchItems[i].FieldNameFrom].month < 10 ? '0' + this._searchModel[this._searchItems[i].FieldNameFrom].month : this._searchModel[this._searchItems[i].FieldNameFrom].month) + '/' + this._searchModel[this._searchItems[i].FieldNameFrom].year;
            }

            if (this._searchModel[this._searchItems[i].FieldNameTo] != null
              && this._searchModel[this._searchItems[i].FieldNameTo] != ''
              && this._searchModel[this._searchItems[i].FieldNameTo] != undefined) {
              this._searchItems[i].Checked = true;
              this._searchItems[i].Value += ' - ' + (this._searchModel[this._searchItems[i].FieldNameTo].day < 10 ? '0' + this._searchModel[this._searchItems[i].FieldNameTo].day : this._searchModel[this._searchItems[i].FieldNameTo].day) + '/' + (this._searchModel[this._searchItems[i].FieldNameTo].month < 10 ? '0' + this._searchModel[this._searchItems[i].FieldNameTo].month : this._searchModel[this._searchItems[i].FieldNameTo].month) + '/' + this._searchModel[this._searchItems[i].FieldNameTo].year;
            }
          }

          if (
            this._searchItems[i].Checked && this._options.Items[i].Type != 'select' && this._options.Items[i].Type != 'ngselect') {
            this._searchValues.push({
              Name: this._searchItems[i].Name,
              Value: this._searchItems[i].Value,
              Hidden: this._searchItems[i].Hidden,
              Permission: this._searchItems[i].Permission,
              Index: i
            });
          }
        }
      }

      if (this._searchValues) {
        this._searchValues = this._searchValues.filter(a => !a.Hidden);
      }

      this.getData();
    }
    this._cd.markForCheck();
  }

  registerOnChange(fn: any): void {
    this._onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this._onTouched = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    this.disabled = isDisabled;
    this._cd.markForCheck();
  }

  searchItemChange(event: any) {
    let value = event.currentTarget.selectedOptions[0].value;
    if (value >= 0) {
      this._searchItems[value].Checked = true;
      this.count++;
    }
  }

  removeItem(index: number) {
    this._searchItems[index].Checked = false;
    this.count--;
  }

  removeSearch(index: number, itemIndex: number) {

    this.removeSearchData(index, itemIndex);
    this.count--;

    this._searchModel = Object.assign({}, this._searchModelView);

    this._onChange(this._searchModel);
    this.changeEvent.emit(this._searchModel);
  }

  removeSearchData(index: number, itemIndex: number) {
    this._searchItems[itemIndex].Checked = false;
    this._searchItems[itemIndex].Value = null;
    if (this._searchItems[itemIndex].Type == 'date') {
      this._searchModelView[this._searchItems[itemIndex].FieldNameFrom] = null;
      this._searchModelView[this._searchItems[itemIndex].FieldNameTo] = null;
    } else if (this._searchItems[itemIndex].Type == 'dateRange') {
      this._searchModelView[this._searchItems[itemIndex].FieldNameFrom] = null;
      this._searchModelView[this._searchItems[itemIndex].FieldNameTo] = null;
    }
    else {
      this._searchModelView[this._searchItems[itemIndex].FieldName] = null;
    }

    this._searchValues.splice(index, 1);

    if (this._searchItems[itemIndex].IsRelation) {
      let isChild = true;
      let indexChild = this._searchItems[itemIndex].RelationIndexTo;
      while (isChild) {

        this._searchItems[indexChild].Checked = false;
        this._searchItems[indexChild].Value = null;

        if (this._searchItems[indexChild].Type == 'date') {
          this._searchModelView[this._searchItems[indexChild].FieldNameFrom] = null;
          this._searchModelView[this._searchItems[indexChild].FieldNameTo] = null;
        }
        else if (this._searchItems[indexChild].Type == 'dateRange') {
          this._searchModelView[this._searchItems[indexChild].FieldNameFrom] = null;
          this._searchModelView[this._searchItems[indexChild].FieldNameTo] = null;
        }
        else {
          this._searchModelView[this._searchItems[indexChild].FieldName] = null;
        }

        let itemRemoveIndex = this._searchValues.findIndex(itemRemove => { return itemRemove.Index == this._searchItems[indexChild].Index });

        if (itemRemoveIndex >= 0) {
          this._searchValues.splice(itemRemoveIndex, 1)
        }

        if (this._searchItems[indexChild].IsRelation) {
          indexChild = this._searchItems[indexChild].RelationIndexTo;
        } else {
          isChild = false;
        }

      }
    }

    if (this._options.Items[itemIndex].IsRelation) {
      this.getDataSelect(this._options.Items[this._options.Items[itemIndex].RelationIndexTo], false, true);
    }
  }

  refresh() {
    this.refreshEvent.emit(true);
  }

  search() {
    this._searchValues = [];
    this.count = 0;
    for (let i = 0; i < this._searchItems.length; i++) {
      // if (this._searchItems[i].Checked && this._searchItems[i].Value) {
      if (this._searchItems[i].Value) {
        this._searchValues.push({
          Name: this._searchItems[i].Name,
          Value: this._searchItems[i].Value,
          Hidden: this._searchItems[i].Hidden,
          Permission: this._searchItems[i].Permission,
          Index: i
        });
        this.count++;
      }
      else {
        this._searchItems[i].Checked = false;
        if (this._searchItems[i].Type == 'date') {
          this._searchModelView[this._searchItems[i].FieldNameFrom] = null;
          this._searchModelView[this._searchItems[i].FieldNameTo] = null;
        }
        else if (this._searchItems[i].Type == 'dateRange') {
          this._searchModelView[this._searchItems[i].FieldNameFrom] = null;
          this._searchModelView[this._searchItems[i].FieldNameTo] = null;
        }
        else {
          this._searchModelView[this._searchItems[i].FieldName] = null;
        }
      }
    }

    if (this._searchValues) {
      this._searchValues = this._searchValues.filter(a => !a.Hidden);
    }

    this._searchModel = Object.assign({}, this._searchModelView);

    this._onChange(this._searchModel);
    this.changeEvent.emit(this._searchModel);
  }

  selectChange(event: any, index: number, name: string) {
    let value = event.currentTarget.selectedOptions[0].value;
    if (value) {
      this._searchItems[index].Value = event.currentTarget.selectedOptions[0].text;
    }
    else {
      this._searchItems[index].Value = ''
    }

    if (this._options.Items[index].IsRelation) {
      this.getDataSelect(this._options.Items[this._options.Items[index].RelationIndexTo], false);
    }
  }

  ngSelectChange(event: any, index: number, valueName: string, displayName: string) {
    let value = null;
    if (event) {
      value = event[valueName];
    }

    if (value != '' && value != null) {
      this._searchItems[index].Value = event[displayName];
    }
    else {
      this._searchItems[index].Value = null;
    }

    if (this._options.Items[index].IsRelation) {
      this.getDataSelect(this._options.Items[this._options.Items[index].RelationIndexTo], false);
    }
  }

  selectChangeExpressionType(event: any, index: number, name: string) {
    let value = event.currentTarget.selectedOptions[0].value;
    if (value && this._searchModelView[name] != null && this._searchModelView[name] != '') {
      this._searchItems[index].Value = event.currentTarget.selectedOptions[0].text + ' ' + this._searchModelView[name];
    }
    else {
      this._searchItems[index].Value = ''
    }
  }

  searchContentChange(contentName: string) {
    this._searchModel[contentName] = this._searchModelView[contentName];
    this._onChange(this._searchModel);
  }

  textChange(index: number, name: string) {
    this._searchItems[index].Value = this._searchModelView[name];
  }

  numberChange(index: number, name: string, nameType: string) {
    if (this._searchModelView[nameType] > 0) {
      this._searchItems[index].Value = this.constants.SearchExpressionTypes[this._searchModelView[nameType] - 1].Name + ' ' + this._searchModelView[name];
    }
    else {
      this._searchItems[index].Value = '';
    }
  }

  numberChangeYear(index: number, name: string) {
    this._searchItems[index].Value = this._searchModelView[name];
  }

  showPopover(popover: any) {
    if (popover.isOpen()) {
      popover.close();
    } else {
      this.count = 0;

      this._searchModelView = Object.assign({}, this._searchModel);

      for (let i = 0; i < this._searchItems.length; i++) {
        this._searchItems[i].Checked = false;
        this._searchItems[i].Value = null;

        for (let j = 0; j < this._searchValues.length; j++) {
          if (this._searchValues[j].Index == i) {
            this._searchItems[i].Checked = true;
            this._searchItems[i].Value = this._searchValues[j].Value;

            if (this._searchItems[i].Value != '' && this._searchItems[i].Value != null && this._searchItems[i].Value != undefined
              && this._searchItems[i].IsRelation) {
              this.getDataSelect(this._options.Items[this._searchItems[i].RelationIndexTo], true);
            }
          }
        }

        if (!this._searchItems[i].Checked) {
          if (this._searchItems[i].Type == 'date') {
            this._searchModelView[this._searchItems[i].FieldNameFrom] = null;
            this._searchModelView[this._searchItems[i].FieldNameTo] = null;
          }
          else {
            this._searchModelView[this._searchItems[i].FieldName] = null;
          }
        }
      }

      popover.open();
    }
  }

  dateChange(index: number, nameFrom: string, nameTo: string) {
    this._searchItems[index].Value = '';
    if (this._searchModelView[nameFrom]) {
      this._searchItems[index].Value = (this._searchModelView[nameFrom].day < 10 ? '0' + this._searchModelView[nameFrom].day : this._searchModelView[nameFrom].day) + '/' + (this._searchModelView[nameFrom].month < 10 ? '0' + this._searchModelView[nameFrom].month : this._searchModelView[nameFrom].month) + '/' + this._searchModelView[nameFrom].year;
    }

    if (this._searchModelView[nameTo]) {
      this._searchItems[index].Value += ' - ' + (this._searchModelView[nameTo].day < 10 ? '0' + this._searchModelView[nameTo].day : this._searchModelView[nameTo].day) + '/' + (this._searchModelView[nameTo].month < 10 ? '0' + this._searchModelView[nameTo].month : this._searchModelView[nameTo].month) + '/' + this._searchModelView[nameTo].year;
    }
  }

  dateRangeChange(index: number, nameFrom: string, nameTo: string) {
    this._searchItems[index].Value = '';
    if (this._searchModelView[nameFrom]) {
      this._searchItems[index].Value = (this._searchModelView[nameFrom]);
    }

    if (this._searchModelView[nameTo]) {
      this._searchItems[index].Value += ' - ' + (this._searchModelView[nameTo]);
    }
  }

  dropdownChange(index: number) {
    let displayName = this._options.Items[index].DisplayName;
    let valueName = this._options.Items[index].ValueName;
    let valueSelect = this._searchModelView[this._options.Items[index].FieldName];
    let selected = this._options.Items[index].Data.filter(function (data: any) {
      if (data[valueName] == valueSelect) {
        return data;
      }
    });

    if (selected && selected.length > 0) {
      this._searchItems[index].Value = selected[0][displayName];
    }
    else {
      this._searchItems[index].Value = null;
    }
  }

  getData() {
    for (let i = 0; i < this._options.Items.length; i++) {
      if (this._options.Items[i].GetData && (!this._options.Items[i].RelationIndexFrom || this._options.Items[i].RelationIndexFrom == 0)) {
        this.getDataSelect(this._options.Items[i], true);
      }
    }
  }

  getDataSelect(item: any, isLoad: boolean, isRemove?: boolean) {
    var parentId = '';

    if (item.RelationIndexFrom >= 0 && this._searchModelView) {
      parentId = this._searchModelView[this._options.Items[item.RelationIndexFrom].FieldName];

      if (!isLoad) {
        this._searchItems[item.Index].Value = null;
        this._searchModelView[item.FieldName] = null;
      }

      // if (parentId) {
      item.GetData(parentId).subscribe(
        (result:any )=> {
          if (result.isStatus) {
            item.Data = result.data;

            if (isLoad) {
              this.setValueDefaut(item);
            }
          }
          else {
            // this.messageService.showMessage(result.message,result.exception);
          }
        }, (error:any) => {
          // this.messageService.showError(error);
        }
      );

      if (item.IsRelation) {
        this.getDataSelect(this._options.Items[item.RelationIndexTo], isLoad, isRemove);
      }

    }
    else {
      item.GetData().subscribe(
        (result:any) => {
          if (result.isStatus) {
            item.Data = result.data;

            if (isLoad) {
              this.setValueDefaut(item);

              if (item.IsRelation) {
                this.getDataSelect(this._options.Items[item.RelationIndexTo], isLoad);
              }
            }
          }
          else {
            // this.messageService.showMessage(result.message,result.exception);
          }
        }, (error:any) => {
          // this.messageService.showError(error);
        }
      );
    }
  }

  setValueDefaut(item: any) {
    var isExist = false;
    for (let i = 0; i < this._searchItems.length; i++) {
      if (item.Index == this._searchItems[i].Id) {
        if (this._searchModel[this._searchItems[i].FieldName] != null
          && this._searchModel[this._searchItems[i].FieldName] != ''
          && this._searchModel[this._searchItems[i].FieldName] != undefined) {

          let valueName = item.ValueName;
          let valueSelect = this._searchModelView[item.FieldName];

          let selected;
          if (item.Data) {
            selected = item.Data.filter(function (data: any) {
              if (data[valueName] == valueSelect) {
                return data;
              }
            });
          }

          if (selected && selected.length > 0) {
            this._searchItems[i].Checked = true;
            this._searchItems[i].Value = selected[0][item.DisplayName];

            isExist = false;
            for (let index = 0; index < this._searchValues.length; index++) {
              if (this._searchValues[index].Index == i) {
                isExist = true;
                this._searchValues[index].Name = this._searchItems[i].Name;
                this._searchValues[index].Value = this._searchItems[i].Value;
              }
            }

            if (!isExist) {
              this._searchValues.push({
                Name: this._searchItems[i].Name,
                Value: this._searchItems[i].Value,
                Hidden: this._searchItems[i].Hidden,
                Permission: this._searchItems[i].Permission,
                Index: i
              });
            }
          }
        }
      }
    }

    if (this._searchValues) {
      this._searchValues = this._searchValues.filter(a => !a.Hidden);
    }
  }
}