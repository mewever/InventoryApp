import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ItemRemove } from './item-remove';

describe('ItemRemove', () => {
  let component: ItemRemove;
  let fixture: ComponentFixture<ItemRemove>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ItemRemove]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ItemRemove);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
