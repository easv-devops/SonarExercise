import {Component, Input, OnInit} from '@angular/core';
import {Box} from "../../models";
import {ActivatedRoute} from "@angular/router";
import {HttpClient} from "@angular/common/http";
import {environment} from "../../environments/environment";
import {firstValueFrom} from "rxjs";

@Component({
  selector: 'app-box-info',
  templateUrl: './box-info.component.html',
  styleUrls: ['./box-info.component.scss'],
})
export class BoxInfoComponent implements OnInit {

  box: Box | undefined;

  constructor(private activatedRoute: ActivatedRoute, private http: HttpClient) {
  }

  ngOnInit() {
    this.loadBoxInfo();
  }

  private async loadBoxInfo() {
    this.activatedRoute.params.subscribe(async (params) => {
      const boxId = params['boxId'];
      if (boxId) {
        const call = this.http.get<Box>(environment.baseUrl + '/api/boxes/' + boxId);
        this.box = await firstValueFrom<Box>(call);
      } else {

      }
    });
  }
}
