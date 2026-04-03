import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { InventoryItemsService, InventoryItem } from '../services/inventoryItemsService.service';

@Component({
  selector: 'app-item-add',
  standalone: false,
  templateUrl: './item-add.html',
  styleUrl: './item-add.css',
})
export class ItemAdd {
  item: InventoryItem = {
    id: 0,
    productCode: '',
    name: '',
    quantity: 0,
    costs: null,
    history: null
  };
  errorMessage: string | null = null;

  constructor(private inventoryItemsService: InventoryItemsService, private router: Router) {
  }

  save(): void {
    this.errorMessage = null;
    this.inventoryItemsService.add(this.item).subscribe(response => {
      if (!response.isSuccessful) {
        this.errorMessage = response.errorMessage;
        return;
      }
      this.router.navigate(['items']);
    });
  }
}
