/*
  This file is automatically generated. Any changes will be overwritten.
  Modify edit-supplier.component.ts instead.
*/
import { LOCALE_ID, ChangeDetectorRef, ViewChild, AfterViewInit, Injector, OnInit, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Subscription } from 'rxjs';

import { DialogService, DIALOG_PARAMETERS, DialogRef } from '@radzen/angular/dist/dialog';
import { NotificationService } from '@radzen/angular/dist/notification';
import { ContentComponent } from '@radzen/angular/dist/content';
import { LabelComponent } from '@radzen/angular/dist/label';
import { ButtonComponent } from '@radzen/angular/dist/button';
import { TemplateFormComponent } from '@radzen/angular/dist/template-form';
import { TextBoxComponent } from '@radzen/angular/dist/textbox';
import { RequiredValidatorComponent } from '@radzen/angular/dist/required-validator';

import { ConfigService } from '../config.service';

import { ConDataService } from '../con-data.service';
import { SecurityService } from '../security.service';

export class EditSupplierGenerated implements AfterViewInit, OnInit, OnDestroy {
  // Components
  @ViewChild('content1') content1: ContentComponent;
  @ViewChild('closeLabel') closeLabel: LabelComponent;
  @ViewChild('closeButton') closeButton: ButtonComponent;
  @ViewChild('label0') label0: LabelComponent;
  @ViewChild('button0') button0: ButtonComponent;
  @ViewChild('form0') form0: TemplateFormComponent;
  @ViewChild('supplierNameLabel') supplierNameLabel: LabelComponent;
  @ViewChild('supplierName') supplierName: TextBoxComponent;
  @ViewChild('supplierNameRequiredValidator') supplierNameRequiredValidator: RequiredValidatorComponent;
  @ViewChild('contactNameLabel') contactNameLabel: LabelComponent;
  @ViewChild('contactName') contactName: TextBoxComponent;
  @ViewChild('addressLabel') addressLabel: LabelComponent;
  @ViewChild('address') address: TextBoxComponent;
  @ViewChild('cityLabel') cityLabel: LabelComponent;
  @ViewChild('city') city: TextBoxComponent;
  @ViewChild('postalCodeLabel') postalCodeLabel: LabelComponent;
  @ViewChild('postalCode') postalCode: TextBoxComponent;
  @ViewChild('countryLabel') countryLabel: LabelComponent;
  @ViewChild('country') country: TextBoxComponent;
  @ViewChild('phoneLabel') phoneLabel: LabelComponent;
  @ViewChild('phone') phone: TextBoxComponent;
  @ViewChild('button3') button3: ButtonComponent;
  @ViewChild('button4') button4: ButtonComponent;

  router: Router;

  cd: ChangeDetectorRef;

  route: ActivatedRoute;

  notificationService: NotificationService;

  configService: ConfigService;

  dialogService: DialogService;

  dialogRef: DialogRef;

  httpClient: HttpClient;

  locale: string;

  _location: Location;

  _subscription: Subscription;

  conData: ConDataService;

  security: SecurityService;
  hasChanges: any;
  canEdit: any;
  supplier: any;
  parameters: any;

  constructor(private injector: Injector) {
  }

  ngOnInit() {
    this.notificationService = this.injector.get(NotificationService);

    this.configService = this.injector.get(ConfigService);

    this.dialogService = this.injector.get(DialogService);

    this.dialogRef = this.injector.get(DialogRef, null);

    this.locale = this.injector.get(LOCALE_ID);

    this.router = this.injector.get(Router);

    this.cd = this.injector.get(ChangeDetectorRef);

    this._location = this.injector.get(Location);

    this.route = this.injector.get(ActivatedRoute);

    this.httpClient = this.injector.get(HttpClient);

    this.conData = this.injector.get(ConDataService);
    this.security = this.injector.get(SecurityService);
  }

  ngAfterViewInit() {
    this._subscription = this.route.params.subscribe(parameters => {
      if (this.dialogRef) {
        this.parameters = this.injector.get(DIALOG_PARAMETERS);
      } else {
        this.parameters = parameters;
      }
      this.load();
      this.cd.detectChanges();
    });
  }

  ngOnDestroy() {
    if (this._subscription) {
      this._subscription.unsubscribe();
    }
  }


  load() {
    this.hasChanges = false;

    this.canEdit = true;

    this.conData.getSupplierBySupplierId(null, this.parameters.SupplierID)
    .subscribe((result: any) => {
      this.supplier = result;
    }, (result: any) => {
      this.canEdit = !(result.status == 400);
    });
  }

  closeButtonClick(event: any) {
    if (this.dialogRef) {
      this.dialogRef.close();
    } else {
      this._location.back();
    }
  }

  button0Click(event: any) {
    this.load()
  }

  form0Submit(event: any) {
    this.conData.updateSupplier(null, this.parameters.SupplierID, event)
    .subscribe((result: any) => {
      if (this.dialogRef) {
        this.dialogRef.close();
      } else {
        this._location.back();
      }
    }, (result: any) => {
      this.hasChanges = result.status == 412;

      this.canEdit = !(result.status == 400);

      this.notificationService.notify({ severity: "error", summary: `Error`, detail: `Unable to update Supplier` });
    });
  }

  button4Click(event: any) {
    if (this.dialogRef) {
      this.dialogRef.close();
    } else {
      this._location.back();
    }
  }
}
