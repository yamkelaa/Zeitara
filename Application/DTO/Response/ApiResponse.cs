namespace Application.DTO.Response;

public class ApiResponse<T>
{
    public T Data { get; private set; }

    public ApiResponse(T data) { 
        Data = data;
    }
}
