import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { CostCenterTreeModel } from '../../../../../Models/Generics/CostCenter';

@Component({
  selector: 'app-cost-center-tree-item',
  templateUrl: './cost-center-tree-item.component.html',
  styleUrl: './cost-center-tree-item.component.css'
})
export class CostCenterTreeItemComponent implements OnInit {

  expanded = false;
  @Input() costCenter: CostCenterTreeModel;

  @Output() selectedCostCenter = new EventEmitter<CostCenterTreeModel>();
  constructor() { }

  ngOnInit(): void {
    this.expanded=(this.costCenter.isSelected&&this.costCenter.children.some(x=>x.isSelected));
  }

  toggleNode(costCenter: CostCenterTreeModel) {
    this.expanded = !this.expanded;
  }
  onEvent(e:Event,costCenter: CostCenterTreeModel,isDelete:boolean=false) {
    event.preventDefault(); 
    event.stopPropagation();
    if (isDelete) {
      costCenter.isDeleteAction = true;
    }
    this.selectCostCenter(costCenter);
  }
  selectCostCenter(costCenter: CostCenterTreeModel) {
    this.selectedCostCenter.emit(costCenter);
  }

  getLevelClass(level: number) {
    // var paddingValue = level
    // if (level > 0)
    //   paddingValue = level * 2;
    return 'padding-right:' + 2 + 'rem !important';
  }
}
