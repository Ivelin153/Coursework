/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { DataExtractComponent } from './data-extract.component';

describe('DataExtractComponent', () => {
  let component: DataExtractComponent;
  let fixture: ComponentFixture<DataExtractComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DataExtractComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DataExtractComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
