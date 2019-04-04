import { SaveVehicle } from './models/vehicle';
import { Http } from '@angular/http';
import { Injectable } from '@angular/core';
import 'rxjs/add/operator/map';

@Injectable()
export class VehicleService {
  private readonly vehicleEndPoint = '/api/vehicles/';

  constructor(private http: Http) {

  }

  getMakes() {
    return this.http.get('/api/makes')
      .map(res => res.json());
  }

  getFeatures() {
    return this.http.get('/api/features')
      .map(res => res.json());

  }

  CreateVehicle(vehicle) {
    return this.http.post(this.vehicleEndPoint, vehicle)
      .map(res => res.json());
  }

  GetVehicle(id) {
    return this.http.get(this.vehicleEndPoint + id).map(res => res.json());
  }

  GetVehicles(filter) {
    return this.http.get(this.vehicleEndPoint + "?" + this.toQueryString(filter)).map(res => res.json());
  }

  UpdateVehicle(vehicle: SaveVehicle) {
    return this.http.put(this.vehicleEndPoint + vehicle.id, vehicle)
      .map(res => res.json());
  }

  DeleteVehicle(id) {
    return this.http.delete(this.vehicleEndPoint + id)
      .map(res => res.json());
  }

  toQueryString(obj) {
    var parts = [];
    for (var property in obj) {
      var val = obj[property];
      if (val != null && val != undefined) {
        parts.push(encodeURIComponent(property) + '=' + encodeURIComponent(val));
      }
    }
    return parts.join('&');

  }

}
