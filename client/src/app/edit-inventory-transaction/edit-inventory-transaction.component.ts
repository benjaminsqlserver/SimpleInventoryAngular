import { Component, Injector } from '@angular/core';
import { EditInventoryTransactionGenerated } from './edit-inventory-transaction-generated.component';

@Component({
  selector: 'page-edit-inventory-transaction',
  templateUrl: './edit-inventory-transaction.component.html'
})
export class EditInventoryTransactionComponent extends EditInventoryTransactionGenerated {
  constructor(injector: Injector) {
    super(injector);
  }
}
