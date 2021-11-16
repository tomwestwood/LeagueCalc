import { Component } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { LeagueFileConverterService } from '../services/league-file-converter.service';

@Component({
  selector: 'app-calculator',
  templateUrl: './calculator.component.html',
  styleUrls: ['./calculator.component.scss']
})
export class CalculatorComponent {
  file: File | undefined;
  leagueTable: MatTableDataSource<any> | undefined;
  displayedColumns: string[] = ['teamPosition', 'teamName', 'goalsScored', 'goalsConceded', 'goalDifference', 'points'];

  constructor(private leagueFileConverterService: LeagueFileConverterService) { }

  uploadFile(event: any) {
    this.file = event.target.files[0];
  }

  calculateLeagueFromFile() {
    //this.loading = !this.loading;
    //console.log(this.file);
    this.leagueFileConverterService.convertFile(this.file).subscribe(
      (event: any) => {
        this.leagueTable = new MatTableDataSource(event.leagueTableEntries);
        //if (typeof (event) === 'object') {

        //  // Short link via api response
        //  this.shortLink = event.link;

        //  this.loading = false; // Flag variable 
        //}
      }
    );
  }
}
