namespace DefaultNamespace
{
    public class InputHandler
    {
        private InputReader _inputReader;
        private void OnEnable()
        {
            _inputReader.OnPauseEvent += PauseGame;
        }

        private void OnDisable()
        {
            _inputReader.OnPauseEvent -= PauseGame;
        }

        private void PauseGame()
        {
        }
    }
}