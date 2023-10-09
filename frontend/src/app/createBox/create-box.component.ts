import {Component} from "@angular/core";
import {FormBuilder, Validators} from "@angular/forms";
import {Box, ResponseDto} from "../../models";
import {HttpClient, HttpErrorResponse} from "@angular/common/http";
import {firstValueFrom} from "rxjs";
import {DataService} from "../data.service";
import {environment} from "../../environments/environment";
import {ModalController, ToastController} from "@ionic/angular";

@Component({
  template: `

    <ion-list>
      <ion-item>
        <ion-select [formControl]="createNewBoxForm.controls.size" data-testid="sizeInput" label="Size" placeholder="Pick size">
          <ion-select-option value="small">small</ion-select-option>
          <ion-select-option value="medium">medium</ion-select-option>
          <ion-select-option value="big">big</ion-select-option>
          <ion-select-option value="large">large</ion-select-option>
        </ion-select>
      </ion-item>
    <ion-item>
      <ion-input [formControl]="createNewBoxForm.controls.weight" data-testid="weightInput"  label="Weight of the box">
      </ion-input>
    </ion-item>
    <ion-item>
      <ion-input [formControl]="createNewBoxForm.controls.price" data-testid="priceInput"  label="Price of the box">
      </ion-input>
    </ion-item>
      <ion-item>
        <ion-select [formControl]="createNewBoxForm.controls.material" data-testid="materialInput" label="Material" placeholder="Pick material">
          <ion-select-option value="paper">paper</ion-select-option>
          <ion-select-option value="plastic">plastic</ion-select-option>
          <ion-select-option value="metal">metal</ion-select-option>
          <ion-select-option value="wood">wood</ion-select-option>
        </ion-select>
      </ion-item>
      <ion-item>
        <ion-select [formControl]="createNewBoxForm.controls.color" data-testid="colorInput" label="Color" placeholder="Pick color">
          <ion-select-option value="clear">clear</ion-select-option>
          <ion-select-option value="red">red</ion-select-option>
          <ion-select-option value="blue">blue</ion-select-option>
          <ion-select-option value="green">green</ion-select-option>
        </ion-select>
      </ion-item>
    <ion-item>
      <ion-input [formControl]="createNewBoxForm.controls.quantity" data-testid="quantityInput"  label="Quantity">

      </ion-input>
    </ion-item>

    <ion-item>
      <ion-button data-testid="submit" [disabled]="createNewBoxForm.invalid" (click)="submit()">Create New Box</ion-button>
    </ion-item>
  </ion-list>

  `
})

export class CreateBoxComponent {

  createNewBoxForm = this.fb.group({
    size: ['', Validators.required, Validators.pattern('(?:small|medium|big|large)')],
    weight: ['', Validators.required],
    price: ['', Validators.required],
    material: ['', Validators.required, Validators.pattern('(?:paper|metal|plastic|wood)')],
    color: ['', Validators.required, Validators.pattern('(?:clear|red|blue|green)')],
    quantity: ['', Validators.required]
  })

  constructor(public fb: FormBuilder, public modalController: ModalController, public http: HttpClient, public dataService: DataService, public toastController: ToastController) {
  }

  async submit() {

    try {
      const observable =     this.http.post<ResponseDto<Box>>(environment.baseUrl + '/api/boxes', this.createNewBoxForm.getRawValue())

      const response = await firstValueFrom(observable);
      this.dataService.boxes.push(response.responseData!);

      const toast = await this.toastController.create({
        message: 'Box was created!',
        duration: 1233,
        color: "success"
      })
      toast.present();
      this.modalController.dismiss();
    } catch (e) {
      if(e instanceof HttpErrorResponse) {
        const toast = await this.toastController.create({
          message: e.error.messageToClient,
          color: "danger"
        });
        toast.present();
      }
    }


  }
}
