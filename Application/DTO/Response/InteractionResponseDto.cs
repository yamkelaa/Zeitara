namespace Application.DTO.Response
{
    public class InteractionResponseDto
    {
        public string Message { get; set; }

        public InteractionResponseDto(string message)
        {
            Message = message;
        }
    }
}
