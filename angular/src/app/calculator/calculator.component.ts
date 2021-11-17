import { OnInit } from '@angular/core';
import { Component } from '@angular/core';
import { CalculatorControllerService } from '../services/calculator-controller.service';

@Component({
  selector: 'app-calculator',
  templateUrl: './calculator.component.html',
  styleUrls: ['./calculator.component.scss']
})
export class CalculatorComponent implements OnInit {
  file: File | undefined;
  loading: boolean = false;

  constructor(private calcControllerService: CalculatorControllerService) { }

  ngOnInit(): void {
    this.calcControllerService.loading.subscribe((isLoading: boolean) => {
      this.loading = isLoading;
    });
  }

  uploadFile(event: any): void {
    this.file = event.target.files[0];
  }

  calculateLeagueFromFile(): void {
    this.calcControllerService.calculateLeagueFromFile(this.file);    
  }
}
