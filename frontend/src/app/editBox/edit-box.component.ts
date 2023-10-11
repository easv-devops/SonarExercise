import {Component} from "@angular/core";
import {FormBuilder, Validators} from "@angular/forms";
import {Box, ResponseDto} from "../../models";
import {HttpClient, HttpErrorResponse} from "@angular/common/http";
import {firstValueFrom} from "rxjs";
import {DataService} from "../data.service";
import {environment} from "../../environments/environment";
import {ModalController, ToastController} from "@ionic/angular";
import {ActivatedRoute} from "@angular/router";

@Component({
  template: `

    <ion-list>
      <ion-item>
        <ion-select [formControl]="editBoxForm.controls.size" data-testid="sizeInput" label="Size"
                    placeholder="{{dataService.currentBox.size}}">
          <ion-select-option value="small">small</ion-select-option>
          <ion-select-option value="medium">medium</ion-select-option>
          <ion-select-option value="big">big</ion-select-option>
          <ion-select-option value="large">large</ion-select-option>
        </ion-select>
      </ion-item>
      <ion-item>
        <ion-input [formControl]="editBoxForm.controls.weight" type="number" data-testid="weightInput"
                   label="Weight of the box">
        </ion-input>
      </ion-item>
      <ion-item>
        <ion-input [formControl]="editBoxForm.controls.price" placeholder="{{dataService.currentBox.price}}" value="{{dataService.currentBox.price}}" type="number" data-testid="priceInput"
                   label="Price of the box">
        </ion-input>
      </ion-item>
      <ion-item>
        <ion-select [formControl]="editBoxForm.controls.material" data-testid="materialInput" label="Material"
                    placeholder="Pick material">
          <ion-select-option value="paper">paper</ion-select-option>
          <ion-select-option value="plastic">plastic</ion-select-option>
          <ion-select-option value="metal">metal</ion-select-option>
          <ion-select-option value="wood">wood</ion-select-option>
        </ion-select>
      </ion-item>
      <ion-item>
        <ion-select [formControl]="editBoxForm.controls.color" data-testid="colorInput" label="Color"
                    placeholder="Pick color">
          <ion-select-option value="clear">clear</ion-select-option>
          <ion-select-option value="red">red</ion-select-option>
          <ion-select-option value="blue">blue</ion-select-option>
          <ion-select-option value="green">green</ion-select-option>
        </ion-select>
      </ion-item>
      <ion-item>
        <ion-input [formControl]="editBoxForm.controls.quantity" type="number" data-testid="quantityInput"
                   label="Quantity">

        </ion-input>
      </ion-item>

      <ion-item>
        <ion-button data-testid="submit" [disabled]="editBoxForm.invalid" (click)="submit()">Update Box
        </ion-button>
      </ion-item>
    </ion-list>

  `
})

export class EditBoxComponent {

  box: Box | undefined;

  editBoxForm = this.fb.group({
    size: [this.dataService.currentBox.size, [Validators.required, Validators.pattern('(?:small|medium|big|large)')]],
    weight: [this.dataService.currentBox.weight, [Validators.required]],
    price: [this.dataService.currentBox.price, [Validators.required]],
    material: [this.dataService.currentBox.material, [Validators.required, Validators.pattern('(?:paper|metal|plastic|wood)')]],
    color: [this.dataService.currentBox.color, [Validators.required, Validators.pattern('(?:clear|red|blue|green)')]],
    quantity: [this.dataService.currentBox.quantity, [Validators.required]]
  })

  constructor(private activatedRoute: ActivatedRoute, public fb: FormBuilder, public modalController: ModalController, public http: HttpClient, public dataService: DataService, public toastController: ToastController) {
  }

  async submit() {
    try {
      const call = this.http.put<Box>(environment.baseUrl + '/api/boxes/' + this.dataService.currentBox.id, this.editBoxForm.value);
      const result = await firstValueFrom<Box>(call);
      let index = this.dataService.boxes.findIndex(b => b.id == this.dataService.currentBox.id)
      this.dataService.boxes[index] = result;
      this.dataService.currentBox = result;
      this.modalController.dismiss();
      const toast = await this.toastController.create({
        message: 'successfully updated',
        duration: 1000,
        color: 'success'
      })
      toast.present();

    } catch (error: any) {
      console.log(error);
      let errorMessage = 'Error';

      if (error instanceof HttpErrorResponse) {
        // The backend returned an unsuccessful response code.
        errorMessage = error.error?.message || 'Server error';
      } else if (error.error instanceof ErrorEvent) {
        // A client-side or network error occurred.
        errorMessage = error.error.message;
      }

      const toast = await this.toastController.create({
        color: 'danger',
        duration: 2000,
        message: errorMessage
      });

      toast.present();
    }

  }

}
