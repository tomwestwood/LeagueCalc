import { Component, Inject, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { Fixture } from '../models/fixture';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { LeagueTableEntry } from '../models/leagueTableEntry';
import { MatPaginator } from '@angular/material/paginator';
import { AfterViewInit } from '@angular/core';
@Component({
  selector: 'app-team-results',
  templateUrl: './team-results.component.html',
  styleUrls: ['./team-results.component.scss']
})
export class TeamResultsComponent implements AfterViewInit {
  results: MatTableDataSource<Fixture> | undefined;
  displayedColumns: string[] = ['kickOff', 'homeTeam', 'awayTeam', 'score', 'referee'];
  @ViewChild(MatPaginator) paginator: MatPaginator;

  constructor(
    public dialogRef: MatDialogRef<TeamResultsComponent>,
    @Inject(MAT_DIALOG_DATA) public data: LeagueTableEntry) {
    this.results = new MatTableDataSource(data.results);
    
  }

  ngAfterViewInit(): void {
    if (this.results)
      this.results.paginator = this.paginator;
  }
}
