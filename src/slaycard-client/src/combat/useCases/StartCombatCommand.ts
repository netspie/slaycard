import CombatRepository from "../entities/CombatRepository";
import { CommandHandler } from "../infrastructure/CommandHandler";
import Result from "../infrastructure/Result";

export class StartCombatCommandHandler extends CommandHandler<StartCombatCommand> {
  combatRepository: CombatRepository;

  constructor(combatRepository: CombatRepository) {
    super();
    this.combatRepository = combatRepository;
  }

  public override async handle(command: StartCombatCommand): Promise<Result> {
    return Result.Failure;
  }
}

export type StartCombatCommand = { userId: string };

export async function handleStartCombatCommand(
  combatRepository: CombatRepository,
  command: StartCombatCommand
): Promise<Result> {

  
  return Result.Failure;
}
