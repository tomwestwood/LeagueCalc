import { OnInit } from '@angular/core';
import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { LeagueTable } from '../../models/leagueTable';
import { LeagueTableEntry } from '../../models/leagueTableEntry';
import { CalculatorControllerService } from '../../services/calculator-controller.service';
import { TeamResultsComponent } from '../../team-results/team-results.component';

@Component({
  selector: 'app-calculator-results',
  templateUrl: './calculator-results.component.html',
  styleUrls: ['./calculator-results.component.scss']
})
export class CalculatorResultsComponent implements OnInit {
  leagueTable: MatTableDataSource<LeagueTableEntry> | undefined;
  loading: boolean = false;
  displayedColumns: string[] = ['teamPosition', 'teamName', 'goalsScored', 'goalsConceded', 'goalDifference', 'points', 'options'];

  constructor(public dialog: MatDialog, private calcControllerService: CalculatorControllerService) { }

  ngOnInit(): void {
    this.calcControllerService.leagueTable.subscribe((leagueTable: LeagueTable | undefined) => {
      this.leagueTable = leagueTable ? new MatTableDataSource(leagueTable?.leagueTableEntries) : undefined;
    });
    this.calcControllerService.loading.subscribe((isLoading: boolean) => {
      this.loading = isLoading;
    });
  }

  showResults(team: LeagueTableEntry) {
    this.dialog.open(TeamResultsComponent, {
      data: team
    });
  }
}
