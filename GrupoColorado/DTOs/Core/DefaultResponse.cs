namespace GrupoColorado.DTOs.Core
{
  public class DefaultResponse
  {
    public DefaultResponse() => this.ClearObjects();

    public int ExitCode { get; set; }
    public string Message { get; set; }
    public int Count { get; set; }
    public bool Success => (ExitCode == 0 || (ExitCode >= 200 && ExitCode <= 299));

    public virtual void ClearObjects() { Count = 0; Message = null; ExitCode = 0; }
  }

  public class DefaultResponse<T> : DefaultResponse
  {
    public DefaultResponse() : base() { }

    public T Data { get; set; }

    public override void ClearObjects() { base.ClearObjects(); Data = default; }
  }
}