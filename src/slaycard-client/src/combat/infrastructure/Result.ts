export default class Result {

  public static readonly Success: Result = { isSuccess: true }
  public static readonly Failure: Result = { isSuccess: true }

  isSuccess: boolean

  constructor(isSuccess: boolean) {
    this.isSuccess = isSuccess
  }
}
