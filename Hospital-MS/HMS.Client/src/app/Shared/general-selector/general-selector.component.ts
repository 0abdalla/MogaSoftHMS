import { Component, EventEmitter, forwardRef, Input, Output } from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { NgbDropdownConfig } from '@ng-bootstrap/ng-bootstrap';
import { GeneralSelectorModel } from '../../Models/Generics/GeneralSelectorModel';

@Component({
  selector: 'app-general-selector',
  templateUrl: './general-selector.component.html',
  styleUrl: './general-selector.component.css',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => GeneralSelectorComponent),
      multi: true,
    },
  ],
})
export class GeneralSelectorComponent {
  @Input() data: GeneralSelectorModel[] = [];
  @Input() placeholder: string = 'اختر';
  @Input() style: string = 'w-50';
  @Input() routeForCreate: string = '';
  @Input() disabled: boolean = false;
  @Input() menuDesign: string = '';
  @Input() showSearch: boolean = false;
  @Input() selectMulti: boolean = false;
  @Input() isCustomDropdown: boolean = false;
  @Output() valueChanged = new EventEmitter<any | any[]>();
  searchText: string = '';
  selectedValue: any = '';
  selectedName: string = '';
  selectedCode: string = '';
  searchFields: string[] = ['name', 'code'];

  private onChange: any = () => { };
  private onTouched: any = () => { };
  constructor(private dropdownConfig: NgbDropdownConfig) { }

  ngOnInit(): void {
    if (this.isCustomDropdown)
      this.dropdownConfig.container = 'body';
    else
      this.dropdownConfig.container = null;
    this.checkCodeExists();
  }

  ngOnChanges(changes: any): void {
    if (changes.data) {
      if (this.selectMulti)
        this.writeValue(this.selectedValues);
      else
        this.writeValue(this.selectedValue);
      this.checkCodeExists();
    }
  }

  checkCodeExists() {
    this.menuDesign = this.data?.some(x => x.code) ? 'table' : ''
  }

  writeValue(value: any): void {
    if (this.selectMulti) {
      if (value)
        this.selectedValues = value;
      var items = this.data?.filter(x => value?.includes(x.value));
      if (items && items.length > 0) {
        this.selectedItems = items.map(x => x.name);
        items.map(x => x.isSelected = true);
      } else {
        this.selectedItems = [];
      }
      this.valueChanged.emit(this.selectedValues);
    }
    else {
      var sName = this.data?.find(x => x.value === value)?.name;
      if (value)
        this.selectedValue = value;
      if (sName)
        this.selectedName = sName;
      else
        this.selectedName = '';
    }
  }


  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  setDisabledState?(isDisabled: boolean): void {
    // Implement if needed
  }

  onInputChange(event: any) {
    const inputValue = event.target.value.toLowerCase();
    this.searchText = inputValue;
  }

  selectOption(option: any): void {
    this.selectedValue = option.value;
    this.onChange(this.selectedValue);
    this.onTouched();
    this.selectedName = option.name;
    this.selectedCode = option.code;
    this.valueChanged.emit(option.value);
  }



  selectedItems: string[] = [];
  selectedValues: string[] = [];
  selectMultiOption(item: any) {
    const index = this.selectedValues.indexOf(item.value);
    var obj = this.data?.find(x => x.value == item.value);
    if (index > -1) {
      this.selectedItems.splice(index, 1);
      this.selectedValues.splice(index, 1);
      if (obj)
        obj.isSelected = false;

    } else {
      this.selectedItems.push(item.name);
      this.selectedValues.push(item.value);
      if (obj)
        obj.isSelected = true;
    }
    this.onChange(this.selectedValues);
    this.onTouched();
    this.valueChanged.emit(this.selectedValues);
  }

  removeSingleSelected() {
    this.selectedName = null;
    this.selectedCode = null;
    this.selectedValue = null;
    this.onChange(null);
    this.valueChanged.emit(null);

  }

  removeMultiSelected() {
    this.data?.map(x => x.isSelected = false);
    this.selectedItems = [];
    this.selectedValues = [];
    this.onChange([]);

  }
}
