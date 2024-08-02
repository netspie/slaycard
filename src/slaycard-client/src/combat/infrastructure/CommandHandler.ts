import Result from "./Result";

export abstract class CommandHandler<TCommand> {
  public abstract handle(command: TCommand): Promise<Result>;
}
