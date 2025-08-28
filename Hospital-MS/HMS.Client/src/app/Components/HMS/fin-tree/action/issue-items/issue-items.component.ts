import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, FormArray, Validators } from '@angular/forms';
import { FinancialService } from '../../../../../Services/HMS/financial.service';
import Swal from 'sweetalert2';
import { FilterModel } from '../../../../../Models/Generics/PagingFilterModel';
export declare var bootstrap:any;
import { StaffService } from '../../../../../Services/HMS/staff.service';
import { todayDateValidator } from '../../../../../validators/today-date.validator';
import html2pdf from 'html2pdf.js';

@Component({
  selector: 'app-issue-items',
  templateUrl: './issue-items.component.html',
  styleUrl: './issue-items.component.css'
})
export class IssueItemsComponent implements OnInit {
  filterForm!:FormGroup;
  addPermissionForm!:FormGroup
   TitleList = ['المخازن','إذن صرف'];
  // 
  adds:any[]=[];
  total:number=0;
  pagingFilterModel:any={
    pageSize:16,
    currentPage:1,
  }
  pagingFilterModelSelect:any={
    pageSize:100,
    currentPage:1,
    filterList:[]
  }
  isFilter:boolean=true;
  allItems: any[] = [];
  suppliers:any[]=[];
  stores:any[]=[];
  purchaseRequests:any[]=[];
  // branchs:any[] = [];
  allRequests:any[]=[]
  jobDeps:any[]=[]
  // 
  username = sessionStorage.getItem('firstName') + '' + sessionStorage.getItem('lastName') ; 
  get today(): string {
    const date = new Date();
    const dateStr = date.toLocaleDateString('ar-EG', {
      weekday: 'long',
      year: 'numeric',
      month: 'long',
      day: 'numeric'
    });
  
    const timeStr = date.toLocaleTimeString('ar-EG', {
      hour: 'numeric',
      minute: '2-digit',
      hour12: true
    });
  
    return `${dateStr} - الساعة ${timeStr}`;
  }
  constructor(private fb:FormBuilder , private financialService:FinancialService , private staffService : StaffService){

    this.filterForm=this.fb.group({
      SearchText:[],
      type:[''],
      responsible:[''],
    })
    this.addPermissionForm = this.fb.group({
      permissionDate: [new Date().toISOString().substring(0, 10) , [todayDateValidator]],
      disbursementRequestId: ['' , Validators.required],
      storeId: ['' , Validators.required],
      jobDepartmentId : ['' , Validators.required],
      items: this.fb.array([
        this.createItemGroup()
      ]),
      notes: ['']
    });    
  }
  ngOnInit(): void {
    this.getItems();
    this.getMaterialIssuePermissions();
    this.getSuppliers();
    this.getStores();
    // this.getPurchaseRequests();
    // this.getBranches();
    this.getAllRequests();
    this.getDeps();
    // 
    this.addPermissionForm.get('disbursementRequestId')?.valueChanges.subscribe(id => {
      if (id) {
        this.financialService.getIssueRequestById(id).subscribe(res => {
          const data = res.results;
          console.log('Data:',data);
          this.addPermissionForm.patchValue({
            jobDepartmentId: data.jobDepartmentId,
            notes: data.notes
          });
          const itemsControl = this.items;
          itemsControl.clear();
          data.items.forEach((item: any) => {
            itemsControl.push(this.createItemGroup(item));
          });
        });
      }
    });
    
  }
  createItemGroup(item: any = null): FormGroup {
    const unitPriceValue = item?.priceAfterTax ?? 1;
    const quantityValue = item?.quantity ?? 1;
    const group = this.fb.group({
      itemId: [item?.itemId ?? '', Validators.required],
      unit: [item?.unit ?? '', Validators.required],
      quantity: [quantityValue, [Validators.required , Validators.min(1)]],
      unitPrice: [{ value: unitPriceValue, disabled: true }, Validators.required],
      totalPrice: [quantityValue * unitPriceValue, Validators.required]
    });
    group.get('quantity')?.valueChanges.subscribe(qty => {
      const price = group.get('unitPrice')?.value || 0;
      group.get('totalPrice')?.setValue(qty * price, { emitEvent: false });
    });
    return group;
  }
  
  get items(): FormArray {
    return this.addPermissionForm.get('items') as FormArray;
  }
  
  addItemRow() {
    this.items.push(this.createItemGroup());
  }
  
  removeItemRow(index: number) {
    if (this.items.length > 1) {
      this.items.removeAt(index);
    }
  }
  applyFilters(){
    this.total=this.adds.length;
  }
  resetFilters(){
    this.filterForm.reset();
    this.applyFilters();
  }
  // 
  onPageChange(event:any){
    this.pagingFilterModel.currentPage=event.page;
    this.pagingFilterModel.pageSize=event.itemsPerPage;
    this.applyFilters();
  }
  // 
  openMainGroup(id:number){
    
  }
  // 
  getMaterialIssuePermissions(){
    this.financialService.getMaterialIssuePermissions(this.pagingFilterModel).subscribe((res:any)=>{
      this.adds=res.results;
      this.total=res.totalCount;
      console.log('Issues:',this.adds);
      this.applyFilters();
    })
  }
  getItems(){
    this.financialService.getItems(this.pagingFilterModelSelect).subscribe((res:any)=>{
      this.allItems=res.results;
      console.log('Items:',this.allItems);
      this.total=res.count;
    })
  }
  getSuppliers(){
    this.financialService.getSuppliers(this.pagingFilterModelSelect).subscribe((res:any)=>{
      this.suppliers=res.results;
      console.log('Suppliers:',this.suppliers);
      this.total=res.count;
    })
  }
  getStores(){
    this.financialService.getStores(this.pagingFilterModelSelect).subscribe((res:any)=>{
      this.stores=res.results;
      console.log('Stores:',this.stores);
      this.total=res.count;
    })
  }
  // getPurchaseRequests(){
  //   this.financialService.getPurchaseRequests(this.pagingFilterModelSelect).subscribe((res:any)=>{
  //     this.purchaseRequests=res.results;
  //     console.log('Purchase Requests:',this.purchaseRequests);
  //     this.total=res.count;
  //   })
  // }
  getAllRequests(){
    this.financialService.getApprovedIssueRequests().subscribe((res:any)=>{
      this.allRequests=res.results;
      console.log('Issue Requests:',this.allRequests);
      this.total=res.count;
    })
  }
  filterChecked(filters: FilterModel[]) {
          this.pagingFilterModel.currentPage = 1;
          this.pagingFilterModel.filterList = filters;
          if (filters.some(i => i.categoryName == 'SearchText'))
            this.pagingFilterModel.searchText = filters.find(i => i.categoryName == 'SearchText')?.itemValue;
          else
            this.pagingFilterModel.searchText = '';
          this.getItems();
  }
  // 
  isEditMode : boolean = false;
  currentMaterialIssuePermissionId: number | null = null;
  printedPermissionData: any;
  documentNumber:number;
  branchName:string;
  storeName:string;
  depName:string;
  addPermission() {
    if (this.addPermissionForm.invalid) {
      this.addPermissionForm.markAllAsTouched();
      return;
    }
  
    const formData = this.addPermissionForm.getRawValue();
  
    if (this.isEditMode && this.currentMaterialIssuePermissionId) {
      this.financialService.updateMaterialIssuePermission(this.currentMaterialIssuePermissionId, formData).subscribe({
        next: () => this.getMaterialIssuePermissions(),
        error: err => console.error('فشل التعديل:', err)
      });
    } else {
      this.financialService.addMaterialIssuePermission(formData).subscribe({
        next: (res: any) => {
          this.getMaterialIssuePermissions();
          console.log(res);
          this.documentNumber = res.results.number;
          this.branchName = res.results.branchName;
          this.storeName = res.results.storeName;
          this.depName = res.results.jobDepartmentName;
          this.printedPermissionData = {
            ...formData,
            permissionDate: res.results.permissionDate,
            restrictionNumber: res.results.dailyRestriction?.restrictionNumber,
            restrictionDate: res.results.dailyRestriction?.restrictionDate,
            accountingGuidanceName: res.results.dailyRestriction?.accountingGuidanceName,
            amount: res.results.dailyRestriction?.amount,
            from: res.results.dailyRestriction?.from,
            to: res.results.dailyRestriction?.to,
            disbursementRequestNumber: this.allRequests.find(ar => ar.id === formData.disbursementRequestId)?.number || '',
            storeName: this.stores.find(s => s.id === formData.storeId)?.name || '',
            depName: this.jobDeps.find(j => j.id === formData.jobDepartmentId)?.name || '',
            itemsNames: formData.items.map((i:any) => {
              const item = this.allItems.find(ai => ai.id === i.itemId);
              return item?.nameAr || '';
            })
          };
          setTimeout(() => this.printPDF(), 300);
          this.addPermissionForm.reset();
        },
        error: err => console.error('فشل الإضافة:', err)
      });
    }
  }
  
  printPDF() {
    const element = document.getElementById('printablePermission');
    if (!element) {
      console.error('العنصر غير موجود');
      return;
    }
    element.style.display = 'block';
    const opt = {
      margin: 0.5,
      filename: `إذن_الصرف_${this.documentNumber}.pdf`,
      image: { type: 'jpeg', quality: 0.98 },
      html2canvas: { scale: 2 },
      jsPDF: { unit: 'in', format: 'a4', orientation: 'portrait' }
    };
    html2pdf().set(opt).from(element).save().then(() => {
      element.style.display = 'none';
    }).catch(err => {
      console.error('حدث خطأ أثناء توليد PDF:', err);
      element.style.display = 'none';
    });
  }
  
  permission!:any;
  editPermission(id: number) {
    this.isEditMode = true;
    this.currentMaterialIssuePermissionId = id;
  
    this.financialService.getMaterialIssuePermissionsById(id).subscribe({
      next: (data) => {
        this.permission=data.results;
        console.log('Permission:',this.permission);
        this.addPermissionForm.patchValue({
          supplierId: this.permission.supplierId,
          documentNumber: this.permission.documentNumber,
          permissionDate: this.permission.permissionDate,
          notes: this.permission.notes,
          items: this.permission.items,
          storeId: this.permission.storeId,
          purchaseRequestId: this.permission.purchaseRequestId
        });
  
        const modal = new bootstrap.Modal(document.getElementById('addPermissionModal')!);
        modal.show();
      },
      error: (err) => {
        console.error('فشل تحميل بيانات إذن الصرف:', err);
      }
    });
  }
  deletePermission(id: number) {
    Swal.fire({
      title: 'هل أنت متأكد؟',
      text: 'هل أنت متأكد من حذف إذن الصرف؟',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'نعم، حذف',
      cancelButtonText: 'إلغاء'
    }).then((result) => {
      if (result.isConfirmed) {
        this.financialService.deleteMaterialIssuePermission(id).subscribe({
          next: () => {
            this.getMaterialIssuePermissions();
          },
          error: (err) => {
            console.error('فشل حذف إذن الصرف:', err);
          }
        });
      }
    });
  }
  // 
  // getBranches(){
  //   this.financialService.getBranches(this.pagingFilterModelSelect).subscribe({
  //     next:(res:any)=>{
  //       this.branchs = res;
  //       console.log('Branchs' , this.branchs);
        
  //     }
  //   })
  // }
  getDeps() {
    this.staffService.getJobDepartment(this.pagingFilterModelSelect.searchText, this.pagingFilterModelSelect.currentPage, this.pagingFilterModelSelect.pageSize, this.pagingFilterModelSelect.filterList).subscribe({
      next: (data: any) => {
        this.jobDeps = data.results;
        console.log('Deps',this.jobDeps);
        this.total = data.totalCount;
      }, error: (err) => {
        console.error('فشل تحميل بيانات الأقسام:', err);
      }
    })
  }
  // getBranchName(id:number){
  //   return this.branchs.find(b => b.id === id)?.nameAr || '';
  // }
  getStoreName(id:number){
    return this.stores.find(s => s.id === id)?.nameAr || '';
  }
  // 
  dataForRePrint:any;
  rePrintPermission(id: number) {
    this.financialService.getMaterialIssuePermissionsById(id).subscribe({
      next: (res: any) => {
        const data = res.results;
        console.log(data);
        
        this.dataForRePrint = {
          ...data,
          accountingGuidanceName : data.dailyRestriction?.accountingGuidanceName,
          from: data.dailyRestriction?.from,
          to: data.dailyRestriction?.to,
          amount: data.dailyRestriction?.amount,
          disbursementRequestNumber: this.allRequests.find(r => r.id === data.disbursementRequestId)?.number || '',
          storeName: this.stores.find(s => s.id === data.storeId)?.name || '',
          jobDepartmentName: this.jobDeps.find(j => j.id === data.jobDepartmentId)?.name || '',
          itemsNames: data.items.map((i: any) => {
            const item = this.allItems.find(ai => ai.id === i.itemId);
            return item?.nameAr || '';
          })
        };
        this.documentNumber = data.documentNumber;
        this.addPermissionForm.patchValue({
          supplierId: data.supplierId,
          documentNumber: data.documentNumber,
          permissionDate: data.permissionDate,
          notes: data.notes,
          items: data.items,
          storeId: data.storeId,
          purchaseRequestId: data.purchaseRequestId
        });
  
        this.rePrintIssueForm();
      },
      error: (err) => {
        console.error('فشل تحميل بيانات إذن الصرف:', err);
      }
    });
  }
  
  rePrintIssueForm() {
    const element = document.getElementById('rePrintablePermission');
    if (!element) {
      console.error('العنصر غير موجود');
      return;
    }
    element.style.display = 'block';
    const opt = {
      margin: 0.5,
      filename: `إذن_الصرف_${this.dataForRePrint?.permissionNumber}.pdf`,
      image: { type: 'jpeg', quality: 0.98 },
      html2canvas: { scale: 2 },
      jsPDF: { unit: 'in', format: 'a4', orientation: 'portrait' }
    };
    html2pdf().set(opt).from(element).save().then(() => {
      element.style.display = 'none';
    }).catch(err => {
      console.error('حدث خطأ أثناء توليد PDF:', err);
      element.style.display = 'none';
    });
  }
  // 
  resetForm() {
    this.addPermissionForm.reset();
    this.isEditMode = false;
    this.currentMaterialIssuePermissionId = null;
  }
}
