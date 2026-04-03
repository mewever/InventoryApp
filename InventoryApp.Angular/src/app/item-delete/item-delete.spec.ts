import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ItemDelete } from './item-delete';

describe('ItemDelete', () => {
  let component: ItemDelete;
  let fixture: ComponentFixture<ItemDelete>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ItemDelete]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ItemDelete);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
