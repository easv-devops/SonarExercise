import {Component, OnInit} from '@angular/core';
import {ModalController, ToastController} from "@ionic/angular";
import {CreateBoxComponent} from "../createBox/create-box.component";

@Component({
  selector: 'app-home',
  templateUrl: 'home.page.html',
  styleUrls: ['home.page.scss'],
})
export class BoxesPage{

  constructor(public modalController: ModalController,
              public toastController: ToastController) {

  }

  async openModal() {
    const modal = await this.modalController.create({
      component: CreateBoxComponent
    });
    modal.present();
  }
}
