import pandas as pd
import numpy as np
import matplotlib.pyplot as plt
import tensorflow as tf
from tensorflow import keras
from sklearn.model_selection import train_test_split
from sklearn.preprocessing import MinMaxScaler
from sklearn.metrics import mean_squared_error, mean_absolute_error, r2_score
from sklearn.preprocessing import StandardScaler
from keras import Sequential
from keras.layers import Dense

# cargar datos simulados
data = pd.read_csv('simulated_pharmacy_sales.csv')
data['Date'] = pd.to_datetime(data['Date'])
data['Day'] = (data['Date'] - data['Date'].min()).dt.days

# crear nuevas características a partir de la fecha
data['Month'] = data['Date'].dt.month
data['Weekday'] = data['Date'].dt.weekday

# lista de medicamentos
medications = data['Medicine'].unique()

# diccionario para almacenar los modelos
models = {}

# normalizador
scaler = StandardScaler()
def predictionData():
    
    # codificar variables categóricas
    data = pd.get_dummies(data, columns=['Store_Location', 'Customer_Age_Group'], drop_first=True)

    # entrenamiento de modelos
    for med in medications:
        print('Entrenando modelo para', med)
        # filtrar datos
        data_med = data[data['Medicine'] == med]
        # separar datos
        X = data_med[['Day', 'Month', 'Weekday', 'Unit_Price', 'Total_Sales', 'Discount', 'Promotion'] + [col for col in data.columns if 'Store_Location' in col or 'Customer_Age_Group' in col]]
        y = data_med['Unit_sold']
        
        # normalizar
        X = scaler.fit_transform(X)
        
        X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=42)
        
        # construir el modelo de red neuronal
        model = Sequential()
        model.add(Dense(64, input_dim=X_train.shape[1], activation='relu'))
        model.add(Dense(32, activation='relu'))
        model.add(Dense(1, activation='linear'))
        
        # compilar el modelo
        model.compile(loss='mean_squared_error', optimizer='adam')
        
        # entrenar el modelo
        model.fit(X_train, y_train, epochs=50, batch_size=10, verbose=1, validation_split=0.2)
        
        y_pred = model.predict(X_test)
        mse = mean_squared_error(y_test, y_pred)
        mae = mean_absolute_error(y_test, y_pred)
        r2 = r2_score(y_test, y_pred)
        
        print(f'Mean Squared Error (MSE): {mse}')
        print(f'Mean Absolute Error (MAE): {mae}')
        print(f'R2 Score: {r2}\n')
        
        models[med] = model

    # predicciones de los próximos 30 días
    future_dates = pd.date_range(data['Date'].max() + pd.Timedelta(days=1), periods=30)
    future_days = pd.DataFrame({
        'Day': np.arange(data['Day'].max() + 1, data['Day'].max() + 31),
        'Month': (data['Date'].max() + pd.to_timedelta(np.arange(1, 31), unit='d')).month,
        'Weekday': (data['Date'].max() + pd.to_timedelta(np.arange(1, 31), unit='d')).weekday,
        'Unit_Price': data['Unit_Price'].mean(),
        'Total_Sales': data['Total_Sales'].mean(),
        'Discount': data['Discount'].mean(),
        'Promotion': 0,  # Asumiendo que no hay promociones en las predicciones futuras
    })

    # codificar variables categóricas para datos futuros
    for col in data.columns:
        if 'Store_Location' in col or 'Customer_Age_Group' in col:
            future_days[col] = 0

    # normalizar
    future_days = scaler.transform(future_days)

    predictions = []
    for med, model in models.items():
        future_quantities = model.predict(future_days)
        pred_df = pd.DataFrame({'Date': future_dates, 'Medicine': med, 'Predicted_Quantity': future_quantities.flatten()})
        predictions.append(pred_df)

    # combinar resultados
    df_predictions = pd.concat(predictions)
    df_predictions.to_csv('predictions.csv')
   

# gráficas de las predicciones
'''plt.figure(figsize=(14, 8))

# Paleta de colores para datos reales y predicciones
colors = plt.cm.tab10.colors

for i, med in enumerate(medications):
    # Datos reales
    data_med = data[data['Medicine'] == med]
    plt.plot(data_med['Date'], data_med['Unit_sold'], label=f'{med} (Actual)', color=colors[i % len(colors)], linewidth=2)

    # Predicciones
    pred_med = df_predictions[df_predictions['Medicine'] == med]
    plt.plot(pred_med['Date'], pred_med['Predicted_Quantity'], '--', label=f'Predicción {med}', color=colors[i % len(colors)], linewidth=2)

# Configuración del gráfico
plt.legend(loc='upper left', bbox_to_anchor=(1, 1))
plt.title('Predicciones de ventas de los siguientes 30 días', fontsize=18, fontweight='bold')
plt.xlabel('Fecha', fontsize=14)
plt.ylabel('Unidades Vendidas', fontsize=14)
plt.xticks(rotation=45)
plt.grid(True, linestyle='--', alpha=0.6)
plt.tight_layout()
plt.show()
'''