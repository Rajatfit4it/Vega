import { ErrorHandler, Inject, NgZone } from "@angular/core";
import { ToastyService } from "ng2-toasty";


export class AppErrorHandler implements ErrorHandler{
    constructor(
        private ngZone: NgZone,
        @Inject(ToastyService) private toasyService: ToastyService){

    }

    handleError(error: any): void {
        console.log(error);
        this.ngZone.run(()=>{
            this.toasyService.error({
                title: 'Error',
                msg: 'An unexpected error' + error,
                theme: 'bootstrap',
                showClose: true,
                timeout: 5000
              });
        });
        
    }

}