
namespace PSSR.Logic.Desciplines
{
    public class PlaceDesciplineDto
    {
        public DesciplineDto Descipline { get; set; }

        public PlaceDesciplineDto(DesciplineDto descipline)
        {
            this.Descipline = descipline;
        }
    }
}
