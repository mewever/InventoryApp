import { Component, inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { InventoryItemsService, ApiResponseTyped, InventoryItem, InventoryItemWithCosts, InventoryItemWithHistory } from '../services/inventoryItemsService.service';

@Component({
  selector: 'app-item-details',
  standalone: false,
  templateUrl: './item-details.html',
  styleUrl: './item-details.css',
})
export class ItemDetails {
  private route = inject(ActivatedRoute);
  inventoryItemId: number = 0;
  inventoryItem: InventoryItem | null = null;
  costs: InventoryItemWithCosts | null = null;
  history: InventoryItemWithHistory | null = null;
  errorMessage: string | null = null;

  constructor(private inventoryItemsService: InventoryItemsService) {
    // Get the id from the route parameter
    var idParm = this.route.snapshot.paramMap.get('id');
    if (idParm != null) {
      this.inventoryItemId = +idParm;
    }

    // Get the item details from the API
    this.inventoryItemsService.details(this.inventoryItemId).subscribe(response => {
      this.inventoryItem = response.response;
    });
  }

  getCosts() {
    // Get the costs for the inventory item from the API
    this.inventoryItemsService.costs(this.inventoryItemId).subscribe(response => {
      this.costs = response.response;
    });
  }

  getHistory() {
    // Get the history for the inventory item from the API
    this.inventoryItemsService.history(this.inventoryItemId).subscribe(response => {
      this.history = response.response;
    });
  }
}
