import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { FilterModel, PagingFilterModel } from '../../../../../Models/Generics/PagingFilterModel';
import { FinancialService } from '../../../../../Services/HMS/financial.service';
import { StaffService } from '../../../../../Services/HMS/staff.service';
import Swal from 'sweetalert2';
declare var bootstrap : any
import html2pdf from 'html2pdf.js';
import { todayDateValidator } from '../../../../../validators/today-date.validator';

@Component({
  selector: 'app-issue-request',
  templateUrl: './issue-request.component.html',
  styleUrl: './issue-request.component.css'
})
export class IssueRequestComponent implements OnInit {
  filterForm!:FormGroup;
  issuseRequestForm!:FormGroup
  TitleList = ['المخازن','طلب صرف'];
  // 
  issues:any[]=[];
  allItems:any[]=[];
  jobDeps:any[]=[];
  total = 0;
  pagingFilterModel : PagingFilterModel = {
    searchText: '',
    currentPage: 1,
    pageSize: 16,
    filterList: []
  }
  pagingFilterModelSelect : PagingFilterModel = {
    currentPage : 1,
    pageSize : 100,
    filterList : []
  };
  isFilter : boolean = true;
  isEditMode : boolean = false;
  constructor(private fb : FormBuilder , private financialService : FinancialService , private staffService : StaffService){
    this.issuseRequestForm = this.fb.group({
      jobDepartmentId: ['' , Validators.required],
      permissionDate: [new Date().toISOString().substring(0, 10) , [todayDateValidator]],
      notes: [''],
      items: this.fb.array([
        this.createItemGroup()
      ]),
    });    
  }
  ngOnInit():void{
    this.getIssues();
    this.getItems();
    this.getDeps();
  }
  //
  createItemGroup(): FormGroup {
    return this.fb.group({
      itemId: ['', Validators.required],
      quantity: [1, [Validators.required , Validators.min(1)]]
    });
  }
  get items(): FormArray {
      return this.issuseRequestForm.get('items') as FormArray;
  }
  addItemRow() {
    this.items.push(this.createItemGroup());
  }
    
  removeItemRow(index: number) {
    if (this.items.length > 1) {
    this.items.removeAt(index);
    }
  }
  getIssues(){
    this.financialService.getIssueRequests(this.pagingFilterModel).subscribe({
      next:(res:any)=>{
        this.issues = res.results
        this.total = res.totalCount
      }
    })
  }
  // 
  getItems(){
    this.financialService.getItems(this.pagingFilterModelSelect).subscribe((res:any)=>{
      this.allItems=res.results;
      console.log(this.allItems);
      this.total=res.count;
    })
  }
  getDeps() {
    this.staffService.getJobDepartment(this.pagingFilterModel.searchText, this.pagingFilterModel.currentPage, this.pagingFilterModel.pageSize, this.pagingFilterModel.filterList).subscribe({
      next: (data: any) => {
        this.jobDeps = data.results;
        console.log(this.jobDeps);
        this.total = data.totalCount;
      }, error: (err) => {
      }
    })
  }
  // 
  openMainGroup(id:number) {}
  onPageChange(page: any) {
    console.log('Page changed to:', page);
    this.pagingFilterModel.currentPage = page.page;
    this.getIssues();
  }
  filterChecked(filters: FilterModel[]) {
    this.pagingFilterModel.currentPage = 1;
    this.pagingFilterModel.filterList = filters;
    if (filters.some(i => i.categoryName == 'SearchText'))
      this.pagingFilterModel.searchText = filters.find(i => i.categoryName == 'SearchText')?.itemValue;
    else
      this.pagingFilterModel.searchText = '';
      this.getIssues();
  }
  // 
  currentIssueId: number | null = null;
  savedIssueData: any;
  documentNumber: number;
  addIssueRequest() {
    if (this.issuseRequestForm.invalid) {
      this.issuseRequestForm.markAllAsTouched();
      return;
    }

    const formData = this.issuseRequestForm.value;
    
    // Enrich items with their names before saving
    const enrichedItems = formData.items.map(item => {
      const fullItem = this.allItems.find(i => i.id === item.itemId);
      return {
        ...item,
        itemNameAr: fullItem?.nameAr || '---',
        itemName: fullItem?.nameEn || '---'
      };
    });

    const enrichedData = {
      ...formData,
      items: enrichedItems
    };

    if (this.isEditMode && this.currentIssueId) {
      this.financialService.updateIssueRequest(this.currentIssueId, formData).subscribe({
        next: () => this.getIssues(),
        error: (err) => console.error('فشل التعديل:', err)
      });
    } else {
      this.financialService.addIssueRequest(formData).subscribe({
        next: (res: any) => {
          this.getIssues();
          
          // Save the enriched data for printing
          this.savedIssueData = enrichedData;
          this.documentNumber = res.results.id;
          
          console.log('Data ready for printing:', this.savedIssueData);
          this.printIssueForm();
          this.issuseRequestForm.reset();
        },
        error: (err) => console.error('فشل الإضافة:', err)
      });
    }
}
  issueRequest!:any;
  editIssueRequest(id: number) {
    this.isEditMode = true;
    this.currentIssueId = id;
  
    this.financialService.getIssueRequestById(id).subscribe({
      next: (data) => {
        this.issueRequest=data.results;
        console.log(this.issueRequest);
        this.issuseRequestForm.patchValue({
          supplierId: this.issueRequest.supplierId,
          documentNumber: this.issueRequest.documentNumber,
          permissionDate: this.issueRequest.permissionDate,
          notes: this.issueRequest.notes,
          items: this.issueRequest.items,
          storeId: this.issueRequest.storeId,
          purchaseOrderId: this.issueRequest.purchaseOrderId
        });
  
        const modal = new bootstrap.Modal(document.getElementById('addPermissionModal')!);
        modal.show();
      },
      error: (err) => {
        console.error('فشل تحميل بيانات طلب الصرف:', err);
      }
    });
  }
  deleteIssueRequest(id: number) {
    Swal.fire({
      title: 'هل أنت متأكد؟',
      text: 'هل أنت متأكد من حذف طلب الصرف؟',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'نعم، حذف',
      cancelButtonText: 'إلغاء'
    }).then((result) => {
      if (result.isConfirmed) {
        this.financialService.deleteIssueRequest(id).subscribe({
          next: () => {
            this.getIssues();
          },
          error: (err) => {
            console.error('فشل حذف طلب الصرف:', err);
          }
        });
      }
    });
  }
  // 
  getDepartmentName(id: number): string {
    const dep = this.jobDeps.find(d => d.id == id)
    return dep?.name || '---';
  }
  getItemName(id: number): string {
    const item = this.allItems.find(i => i.id == id);
    return item?.nameAr || '---';
  }
  getStatusName(type: string): string {
    const map: { [key: string]: string } = {
      Approved: 'تم الموافقة',
      Pending: 'قيد الانتظار',
      Rejected: 'مرفوض'
    };
    return map[type] || type;
  }
  getStatusColor(type: string): string {
    const map: { [key: string]: string } = {
      Approved: '#00FF00',
      Pending: '#FFA500',
      Rejected: '#FF0000'
    };
    return map[type] || '#000000';
  }
  printIssueForm() {
    const element = document.getElementById('printableIssue');
    if (!element) {
      console.error('العنصر غير موجود');
      return;
    }
    element.style.display = 'block';
    const opt = {
      margin: 0.5,
      filename: 'طلب_الصرف.pdf',
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
}
