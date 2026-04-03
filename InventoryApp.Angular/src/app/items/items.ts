import { Component } from '@angular/core';
import { InventoryItemsService, ApiResponseTyped, InventoryItem } from '../services/inventoryItemsService.service';

@Component({
  selector: 'app-items',
  standalone: false,
  templateUrl: './items.html',
  styleUrl: './items.css',
})

export class Items {
  inventoryItems: InventoryItem[] | null = null;

  constructor(private inventoryItemsService: InventoryItemsService) {
  }

  ngOnInit(): void {
    this.inventoryItemsService.list().subscribe(response => {
      var apiResponse: ApiResponseTyped<InventoryItem[]> = {
        isSuccessful: false,
        errorMessage: null,
        response: null
      };
      apiResponse = response;
      this.inventoryItems = apiResponse.response;
    });
  }
}
