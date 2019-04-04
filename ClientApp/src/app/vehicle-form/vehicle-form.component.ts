import { SaveVehicle, Vehicle } from './../models/vehicle';
import { VehicleService } from '../vehicle.service';
import { Component, OnInit } from '@angular/core';
import { ToastyService } from 'ng2-toasty';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/forkJoin';
import * as _ from 'underscore';

@Component({
  selector: 'app-vehicle-form',
  templateUrl: './vehicle-form.component.html',
  styleUrls: ['./vehicle-form.component.css']
})
export class VehicleFormComponent implements OnInit {
  makes: any[];
  vehicle: SaveVehicle = {
    id: 0,
    makeId: 0,
    modelId: 0,
    isRegistered: false,
    features: [],
    contact: {
      name: "",
      phone: "",
      email: ""
    }
  };
  models: any[];
  features: any[];
  constructor(private vehicleService: VehicleService,
    private toasyService: ToastyService,
    private route: ActivatedRoute,
    private router: Router
  ) {

    this.route.params.subscribe(p => {
      this.vehicle.id = +p['id'] || 0;
    });
  }

  ngOnInit() {
    var sources = [
      this.vehicleService.getMakes(),
      this.vehicleService.getFeatures()
    ];
    if (this.vehicle.id) {
      sources.push(this.vehicleService.GetVehicle(this.vehicle.id));
    }
    Observable.forkJoin(sources).subscribe(data => {
      this.makes = data[0];
      this.features = data[1];
      if (this.vehicle.id) {
        this.setVehicle(data[2]);
        this.populateModels();
      }
    }, err => {
      if (err.status == 404) {
        this.router.navigate(['/home']);
      }
    });
  }

  setVehicle(v: Vehicle) {
    this.vehicle.id = v.id;
    this.vehicle.modelId = v.model.id;
    this.vehicle.makeId = v.make.id;
    this.vehicle.isRegistered = v.isRegistered;
    this.vehicle.contact = v.contact;
    this.vehicle.features = _.pluck(v.features, 'id');
  }

  onMakeChange() {
    this.populateModels();
    delete this.vehicle.modelId;
  }

  private populateModels() {
    var selectedmodel = this.makes.find(item => item.id == this.vehicle.makeId);
    this.models = selectedmodel ? selectedmodel.models : [];
  }

  onChangeFeature(id, $event) {
    if ($event.target.checked) {
      this.vehicle.features.push(id);
    }
    else {
      var index = this.vehicle.features.indexOf(id);
      this.vehicle.features.splice(index, 1);
    }
  }

  submit() {
    if (this.vehicle.id) {
      this.vehicleService.UpdateVehicle(this.vehicle).subscribe(res => {
        this.toasyService.success({
          title: "Success",
          msg: "Record Updated Successfully!!!",
          showClose: true,
          theme: 'bootstrap',
          timeout: 5000
        });
        this.router.navigate(['/vehicles/']);
      });
    }
    else {
      this.vehicleService.CreateVehicle(this.vehicle).subscribe(res => {
        this.toasyService.success({
          title: "Success",
          msg: "Record Created Successfully!!!",
          showClose: true,
          theme: 'bootstrap',
          timeout: 5000
        });
        this.router.navigate(['/vehicles/']);
      });

    }

  }

  delete() {
    this.vehicleService.DeleteVehicle(this.vehicle.id)
      .subscribe(res => {
        if (confirm("Are you sure?")) {
          this.router.navigate(['/home']);
        }

      });

  }

}
