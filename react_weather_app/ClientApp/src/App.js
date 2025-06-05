import { useState } from "react";
import "./custom.css";

function App() {
  const [city, setCity] = useState("");
  const [weather, setWeather] = useState(null);
  const [error, setError] = useState("");

  const fetchWeather = async () => {
    setError("");
    try {
      const res = await fetch(`/weatherforecast?city=${city}`);
      if (!res.ok) throw new Error("API error");
      const data = await res.json();
      console.log(data);
      setWeather(data);
    } catch (err) {
      setError("Could not fetch weather. Try another city");
      setWeather(null);
    }
  };

  return (
    <div className="bg-blue-500">
      <div className="p-4 max-w-md mx-auto ">
        <h1 className="text-2xl font-bold mb-4 ">Weather App</h1>

        <div className="flex gap-2 mb-4">
          <input
            className="border p-2 flex-1"
            type="text"
            placeholder="Enter city"
            value={city}
            onChange={(e) => setCity(e.target.value)}
          />
          <button
            className="text-black px-4 py-2 rounded"
            onClick={fetchWeather}
          >
            Search
          </button>
        </div>

        {error && <p className="text-red-600">{error}</p>}

        {weather && (
          <div className="border p-4 rounded bg-blue-400">
            <p>
              <strong>City:</strong> {weather.city}
            </p>
            <p>
              <strong>Date:</strong> {weather.date}
            </p>
            <p>
              <strong>Temperature:</strong> {weather.temperatureF} °F
            </p>
            <p>
              <strong>Condition:</strong> {weather.summary}
            </p>
          </div>
        )}
      </div>
    </div>
  );
}

export default App;
