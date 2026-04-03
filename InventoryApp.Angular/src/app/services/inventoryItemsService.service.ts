import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class InventoryItemsService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  list(): Observable<ApiResponseTyped<InventoryItem[]>> {
    return this.http.get<ApiResponseTyped<InventoryItem[]>>(`${this.apiUrl}/api/items/list`);
  }

  details(id: number): Observable<ApiResponseTyped<InventoryItem>> {
    var request: GetInventoryItemRequest = {
      id: id
    };
    return this.http.post<ApiResponseTyped<InventoryItem>>(`${this.apiUrl}/api/items/details`, request);
  }

  add(item: InventoryItem): Observable<ApiResponseTyped<AddInventoryItemResponse>> {
    return this.http.post<ApiResponseTyped<AddInventoryItemResponse>>(`${this.apiUrl}/api/add`, item);
  }

  update(item: InventoryItem): Observable<ApiResponse> {
    return this.http.post<ApiResponse>(`${this.apiUrl}/api/items/update`, item);
  }

  delete(id: number): Observable<ApiResponse> {
    var request: InventoryItemDeleteRequest = {
      id: id
    };
    return this.http.post<ApiResponse>(`${this.apiUrl}/api/items/delete`, request);
  }

  receive(
    inventoryItemId: number,
    referenceNumber: string,
    description: string,
    quantityReceived: number,
    amountPaid: number
  ): Observable<ApiResponse> {
    var request: ReceiveInventoryRequest = {
      inventoryItemId: inventoryItemId,
      referenceNumber: referenceNumber,
      description: description,
      quantityReceived: quantityReceived,
      amountPaid: amountPaid
    };
    return this.http.post<ApiResponse>(`${this.apiUrl}/api/items/receive`, request)
  }

  remove(
    inventoryItemId: number,
    referenceNumber: string,
    description: string,
    quantityRemoved: number
  ): Observable<ApiResponse> {
    var request: RemoveInventoryRequest = {
      inventoryItemId: inventoryItemId,
      referenceNumber: referenceNumber,
      description: description,
      quantityRemoved: quantityRemoved
    };
    return this.http.post<ApiResponse>(`${this.apiUrl}/api/items/remove`, request)
  }

  costs(id: number): Observable<ApiResponseTyped<InventoryItemWithCosts>> {
    var request: InventoryItemCostsRequest = {
      id: id
    };
    return this.http.post<ApiResponseTyped<InventoryItemWithCosts>>(`${this.apiUrl}/api/items/costs`, request);
  }

  history(id: number): Observable<ApiResponseTyped<InventoryItemWithHistory>> {
    var request: InventoryItemHistoryRequest = {
      id: id
    };
    return this.http.post<ApiResponseTyped<InventoryItemWithHistory>>(`${this.apiUrl}/api/items/history`, request);
  }
}

export class AddInventoryItemResponse {
  id: number = 0;
}

export class ApiResponse {
  isSuccessful: boolean = false;
  errorMessage: string | null = null;
}

export class ApiResponseTyped<T> {
  isSuccessful: boolean = false;
  errorMessage: string | null = null;
  response: T | null = null;
}

export class GetInventoryItemRequest {
  id: number = 0;
}

export class HistoryItem {
  id: number = 0;
  inventoryItemId: number = 0;
  referenceNumber: string = '';
  description: string = '';
  quantityChange: number = 0;
  valueChange: number = 0;

  costs: HistoryItemCost[] | null = null;
}
export class HistoryItemCost {
  id: number = 0;
  inventoryItemCostId: number = 0;
  quantityChange: number = 0;
  unitValue: number = 0;
  valueChange: number = 0;
}

export class InventoryHistoryListItem {
  historyDate: Date = new Date(1900, 1, 1);
  referenceNumber: string = '';
  historyQuantity: number = 0;
  historyCost: number = 0;
  costDate: Date = new Date(1900, 1, 1);
  costQuantity: number = 0;
  costUnitCost: number = 0;
}

export class InventoryItem {
  id: number = 0;
  productCode: string = '';
  name: string = '';
  quantity: number = 0;

  costs: InventoryItemCost[] | null = null;
  history: HistoryItem[] | null = null;
}

export class InventoryItemCost {
  id: number = 0;
  inventoryItemId: number = 0;
  receivedTimestamp: Date = new Date();
  originalCost: number = 0;
  originalQuantity: number = 0;
  currentCost: number = 0;
  currentQuantity: number | null = null;
}

export class InventoryItemCostsRequest {
  id: number = 0;
}

export class InventoryItemDeleteRequest {
  id: number = 0;
}

export class InventoryItemHistoryRequest {
  id: number = 0;
}

export class InventoryItemWithCosts extends InventoryItem {
  totalCost: number = 0;
}

export class InventoryItemWithHistory extends InventoryItem {
  totalCost: number = 0;
  listItems: InventoryHistoryListItem[] = [];
}

export class ReceiveInventoryRequest {
  inventoryItemId: number = 0;
  referenceNumber: string = '';
  description: string = '';
  quantityReceived: number = 0;
  amountPaid: number = 0;
}

export class RemoveInventoryRequest {
  inventoryItemId: number = 0;
  referenceNumber: string = '';
  description: string = '';
  quantityRemoved: number = 0;
}
