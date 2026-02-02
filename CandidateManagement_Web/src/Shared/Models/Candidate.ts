import { Skill } from "./Skill";

export interface Candidate {
  id?: number;
  name: string;
  nickname: string;
  email: string;
  yearsOfExperience: number;
  maxNumSkills?: number;
  skills?: Skill[];
}

