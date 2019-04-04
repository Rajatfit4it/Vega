import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { VehicleService } from '../vehicle.service';
import { ToastyService } from 'ng2-toasty';
import { Observable } from 'rxjs/Observable';
import { Vehicle } from './../models/vehicle';
import { PhotosService } from '../photos.service';

@Component({
  selector: 'app-view-vehicle',
  templateUrl: './view-vehicle.component.html',
  styleUrls: ['./view-vehicle.component.css']
})
export class ViewVehicleComponent implements OnInit {
  vehicle: any;
  vehicleId: number;
  IsBasic: boolean = true;
  photos: any[];
  @ViewChild('fileInput') fileInput: ElementRef;
  constructor(private vehicleService: VehicleService,
    private toasyService: ToastyService,
    private route: ActivatedRoute,
    private router: Router,
    private photoService: PhotosService
  ) {

    route.params.subscribe(p => {
      this.vehicleId = +p['id'];
      if (isNaN(this.vehicleId) || this.vehicleId <= 0) {
        router.navigate(['/vehicles']);
        return;
      }
    });
  }

  ngOnInit() {
    this.photoService.getPhotos(this.vehicleId)
    .subscribe(data=> this.photos = data);

    this.vehicleService.GetVehicle(this.vehicleId)
      .subscribe(data => {
        this.vehicle = data;
      }, err => {
        if (err.status == 404) {
          this.router.navigate(['/vehicles']);
        }
      });
  }

  delete() {
    if (confirm("Are you sure?")) {
      this.vehicleService.DeleteVehicle(this.vehicle.id)
        .subscribe(res => {
          this.router.navigate(['/vehicles']);
        });
    }
  }

  edit() {
    this.router.navigate(['/vehicles/edit/' + this.vehicle.id]);

  }

  toggleTab(isBasic) {
    this.IsBasic = isBasic;
  }
  uploadPhoto() {
    var nativeElement: HTMLInputElement = this.fileInput.nativeElement;
    this.photoService.uploadPhoto(this.vehicleId, nativeElement.files[0])
    .subscribe(res=> {
      this.photos.push(res);
    });
  }

  

}
