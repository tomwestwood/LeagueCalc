import { Fixture } from "./fixture";

export class LeagueTableEntry {
  teamPosition: number;
  teamName: string;
  goalsScored: number;
  goalsConceded: number;
  goalDifference: number;
  points: number;
  results: Fixture[];
}
