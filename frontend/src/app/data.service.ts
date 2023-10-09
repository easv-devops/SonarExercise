import {Injectable} from "@angular/core";
import {Box} from "../models";
import {Observable, of} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class DataService {

  public boxes: Box[] = [];
  public currentBox: Box = {};


}
