import { VehicleService } from './../vehicle.service';
import { Vehicle, KeyValuePair } from './../models/vehicle';
import { Component, OnInit } from '@angular/core';
import { query } from '@angular/core/src/render3/instructions';

@Component({
  selector: 'app-vehicle-list',
  templateUrl: './vehicle-list.component.html',
  styleUrls: ['./vehicle-list.component.css']
})
export class VehicleListComponent implements OnInit {
private readonly PAGE_SIZE = 3;
  queryResult: any = {};
  makes: KeyValuePair[];
  query: any = {
    pageSize:3
  }
  columns= [
    { title: 'Id' },
    { title: 'Contact Name', key: 'contactName', isSortable: true },
    { title: 'Make', key: 'make', isSortable: true },
    { title: 'Model', key: 'model', isSortable: true },
    { }
  ];

  //makeId: number;
  constructor(private vehicleService: VehicleService) { }

  ngOnInit() {
    this.populateVehicles();
    this.vehicleService.getMakes().subscribe(makes => {
      this.makes = makes;
    });
  }

  populateVehicles() {
    this.vehicleService.GetVehicles(this.query)
      .subscribe(res => this.queryResult = res);
  }

  onFilterChange() {
    
    this.query.page = 1;
    this.populateVehicles();

  }
  onReset() {
    this.query = {
      page: 1,
      pageSize : this.PAGE_SIZE
    };
    this.populateVehicles();
  }

  sortBy(columnName) {
    if (this.query.sortBy == columnName) {
      this.query.isSortAscending = !this.query.isSortAscending;
    } else {
      this.query.sortBy = columnName;
      this.query.isSortAscending = true;
    }
    console.log(this.query);
    this.populateVehicles();
  }

  onPageChange(page){
    this.query.page = page;
    this.populateVehicles();
  }

}
