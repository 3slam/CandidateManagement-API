import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Skill } from "../Models/Skill";
import { constants } from "../constants";
 

@Injectable({ providedIn: 'root' })
export class SkillService {
  constructor(private http: HttpClient) {}

  addSkill(candidateId: number, skill: Skill) {
    return this.http.post(
      `${constants.apiUrl}/candidates/${candidateId}/skills`,
      skill
    );
  }
}